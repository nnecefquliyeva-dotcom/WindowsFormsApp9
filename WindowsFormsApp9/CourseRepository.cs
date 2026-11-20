using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public class CourseRepository
    {
        string connectionString =
            @"Server=.\SQLEXPRESS;Database=StudentGradesDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Course> GetAll()
        {
            List<Course> courses = new List<Course>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseId, CourseName, Credits FROM Courses";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course();

                    course.Id = Convert.ToInt32(reader["CourseId"]);
                    course.Title = reader["CourseName"].ToString();
                    course.Credits = Convert.ToInt32(reader["Credits"]);

                    courses.Add(course);
                }
            }

            return courses;
        }

        public void Add(Course course)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Courses (CourseName, Credits) VALUES (@Title, @Credits)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Credits", course.Credits);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void Update(Course course)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Courses SET CourseName = @Title, Credits = @Credits WHERE CourseId = @Id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", course.Id);
                cmd.Parameters.AddWithValue("@Title", course.Title);
                cmd.Parameters.AddWithValue("@Credits", course.Credits);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Courses WHERE CourseId = @Id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int Count()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Courses";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
