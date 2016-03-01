using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp
{
  public class Course
  {
    private int _id;
    private string _courseName;
    private int _courseNumber;

    public Course(string courseName, int courseNumber, int Id = 0)
    {
      _id = Id;
      _courseName = courseName;
      _courseNumber = courseNumber;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if(!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.GetId() == newCourse.GetId();
        bool courseNameEquality = this.GetCourseName() == newCourse.GetCourseName();
        bool courseNumberEquality = this.GetCourseNumber() == newCourse.GetCourseNumber();
        return (idEquality && courseNameEquality && courseNumberEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetCourseName()
    {
      return _courseName;
    }
    public void SetCourseName(string NewCourseName)
    {
      _courseName = NewCourseName;
    }
    public int GetCourseNumber()
    {
      return _courseNumber;
    }
    public void SetCourseNumber(int NewCourseNumber)
    {
      _courseNumber = NewCourseNumber;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
      cmd.ExecuteNonQuery();
    }
    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        string courseName = rdr.GetString(1);
        int courseNumber = rdr.GetInt32(2);
        Course newCourse = new Course(courseName, courseNumber, courseId);
        allCourses.Add(newCourse);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allCourses;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO courses (coursename, coursenumber) OUTPUT INSERTED.id VALUES (@CourseName, @CourseNumber);", conn);

      SqlParameter courseNameParameter = new SqlParameter();
      courseNameParameter.ParameterName = "@CourseName";
      courseNameParameter.Value = this.GetCourseName();

      SqlParameter courseNumberParameter = new SqlParameter();
      courseNumberParameter.ParameterName = "@CourseNumber";
      courseNumberParameter.Value = this.GetCourseNumber();

      cmd.Parameters.Add(courseNameParameter);
      cmd.Parameters.Add(courseNumberParameter);

      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }


  }
}
