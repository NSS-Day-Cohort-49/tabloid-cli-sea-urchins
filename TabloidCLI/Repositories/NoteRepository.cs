using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI.Repositories
{
    public class NoteRepository : DatabaseConnector, IRepository<Note>
    {
        public NoteRepository(string connectionString) : base(connectionString) { }

        public List<Note> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                        Title,
                                        Content,
                                        CreateDateTime
                                        FROM Note";

                    List<Note> notes = new List<Note>();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        notes.Add(note);
                    }

                    reader.Close();

                    return notes;
                }
            }
        }
        public Note Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id as BlogId,
                                        b.Title,
                                        b.URL,
                                        t.Id as TagId,
                                        t.Name
                                        FROM Blog b
                                            LEFT JOIN BlogTag at on b.Id = at.BlogId
                                            LEFT JOIN Tag t on t.Id = at.TagId
                                        WHERE b.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Note note = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (note == null)
                        {
                            note = new Note()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("Title"))
                            };
                        }
                        //if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                        //{
                        //    note.Tags.Add(new Tag()
                        //    {
                        //        Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                        //        Name = reader.GetString(reader.GetOrdinal("Name")),
                         //   });
                       // }

                    }
                    reader.Close();

                    return note;
                }
            }
        }
        public void Insert(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Note (Title, Content, CreateDateTime, PostId )
                                        VALUES (@title, @content, GETDATE(), @postId)";


                    cmd.Parameters.AddWithValue("@title", note.Title);
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@postId", note.PostId);

                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void Update(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Note
                                            SET Title = @title,
                                                Content = @content
                                           WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@title", note.Title);
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@id", note.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Note
                                            WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
