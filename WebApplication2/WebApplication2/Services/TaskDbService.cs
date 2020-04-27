using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using WebApplication2.DTOs.Requests;
using WebApplication2.DTOs.Responses;
using WebApplication2.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Services
{
    public class TaskDbService : IDbService
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s18511;Integrated Security=True";

        public List<ProjectInfoResponse> GetInfoAboutProject(string projectId)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText =
                    "SELECT t.IdTask, t.Name, t.Description, t.Deadline, t.IdProject, t2.Name FROM dbo.Task t INNER JOIN dbo.TaskType t2 ON t.IdTaskType = t2.IdTaskType WHERE t.idProject = @projectId ORDER BY t.Deadline DESC"
            };
            command.Parameters.AddWithValue("projectId", projectId);
            var reader = command.ExecuteReader();
            var projects = new List<ProjectInfoResponse>();
            while (reader.Read())
            {
                projects.Add(new ProjectInfoResponse
                {
                    Deadline = DateTime.Parse(reader["Deadline"].ToString()),
                    Description = reader["Description"].ToString(),
                    IdTask = reader["IdTask"].ToString(),
                    IdTeam = reader["IdProject"].ToString(),
                    TaskName = reader["t.Name"].ToString(),
                    TaskTypeName = reader["t2.Name"].ToString()
                });
            }

            return projects;
        }

        public void addTaskRequestAndTaskTypeIfNotExists(AddTaskRequest taskRequest, Task taskType)
        {
            using var connection = new SqlConnection(ConnectionString);
            using var command = new SqlCommand
            {
                Connection = connection
            };
            using var tran = connection.BeginTransaction();
            try
            {
                if (!ExistsInDb(taskType, command))
                {
                    AddToTaskTypeToDb(taskType, command);
                }

                AddTaskRequestToDb(taskRequest, taskType, command);
                tran.Commit();
            }
            catch (SqlException e)
            {
                tran.Rollback();

                throw e;
            }
        }

        private void AddTaskRequestToDb(AddTaskRequest taskRequest, Task taskType, SqlCommand command)
        {
            command.CommandText =
                "INSERT INTO 18511.dbo.Task(Name, Description, Deadline, IdProject, IdTaskType, IdAssignedTo, IdCreator) VALUES (@name,@description,@idProject,@idTaskType,@idAssignedTo,@idCreator,@Deadline)";
            command.Parameters.AddWithValue("name", taskRequest.Name);
            command.Parameters.AddWithValue("description", taskRequest.Description);
            command.Parameters.AddWithValue("idProject", taskRequest.IdProject);
            command.Parameters.AddWithValue("idTaskType", taskType.IdTaskType);
            command.Parameters.AddWithValue("IdAssignedTo", taskRequest.IdAssignedTo);
            command.Parameters.AddWithValue("IdCreator", taskRequest.IdCreator);
            command.Parameters.AddWithValue("Deadline", taskRequest.Deadline.ToString(CultureInfo.CurrentCulture));
        }

        private void AddToTaskTypeToDb(Task taskType, SqlCommand command)
        {
            command.CommandText = "insert into 18511.dbo.TaskType VALUES (@taskTypeId,@taskTypeName)";
            command.Parameters.AddWithValue("taskTypeId", taskType.IdTaskType);
            command.Parameters.AddWithValue("taskTypeName", taskType.Name);
            command.ExecuteNonQuery();
        }

        private bool ExistsInDb(Task taskType, SqlCommand command)
        {
            command.CommandText = "select * from 18511.dbo.TaskType where IdTaskType = @idTaskType and Name = @name";
            command.Parameters.AddWithValue("idTaskType", taskType.IdTaskType);
            command.Parameters.AddWithValue("name", taskType.Name);
            var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}