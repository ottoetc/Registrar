using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RegistrarApp
{
  public class Student
  {
    private int _id;
    private string _studentName;
    private DateTime _enrollmentDate;

    public Student(string studentName, DateTime enrollmentDate, int Id = 0)
    {
      _id = Id;
      _studentName = studentName;
      _enrollmentDate = enrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if(!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.GetId() == newStudent.GetId();
        bool studentNameEquality = this.GetStudentName() == newStudent.GetStudentName();
        bool enrollmentDateEquality = this.GetEnrollmentDate() == newStudent.GetEnrollmentDate();
        return (idEquality && studentNameEquality && enrollmentDateEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetStudentName()
    {
      return _studentName;
    }
    public void SetStudentName(string NewStudentName)
    {
      _studentName = NewStudentName;
    }
    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }
    public void SetEnrollmentDate(DateTime NewEnrollmentDate)
    {
      _enrollmentDate = NewEnrollmentDate;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int personId = rdr.GetInt32(0);
        string studentName = rdr.GetString(1);
        DateTime enrollmentDate = rdr.GetDateTime(2);
        Student newStudent = new Student(studentName, enrollmentDate, personId);
        allStudents.Add(newStudent);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (studentname, enrollmentdate) OUTPUT INSERTED.id VALUES (@StudentName, @EnrollmentDate);", conn);

      SqlParameter studentNameParameter = new SqlParameter();
      studentNameParameter.ParameterName = "@StudentName";
      studentNameParameter.Value = this.GetStudentName();

      SqlParameter enrollmentDateParameter = new SqlParameter();
      enrollmentDateParameter.ParameterName = "@EnrollmentDate";
      enrollmentDateParameter.Value = this.GetEnrollmentDate();

      cmd.Parameters.Add(studentNameParameter);
      cmd.Parameters.Add(enrollmentDateParameter);

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

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId;", conn);
      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = id.ToString();
      cmd.Parameters.Add(studentIdParameter);
      rdr = cmd.ExecuteReader();

      int foundStudentId = 0;
      string foundStudentName = null;
      DateTime foundEnrollmentDate = new DateTime();

      while(rdr.Read())
      {
        foundStudentId = rdr.GetInt32(0);
        foundStudentName = rdr.GetString(1);
        foundEnrollmentDate = rdr.GetDateTime(2).Date;
      }
      Student foundStudent = new Student(foundStudentName, foundEnrollmentDate, foundStudentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students WHERE id = @StudentId; DELETE FROM stedents_courses WHERE student_id = @StudentId;", conn);

      SqlParameter studentIdParameter = new SqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = this.GetId();

      cmd.Parameters.Add(studentIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
