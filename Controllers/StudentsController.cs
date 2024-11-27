using lab12.Models;
using lab12.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public StudentsController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Student> GetAll()
        {
            return _context.Students.Where(s => s.Active).ToList(); 
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpGet]
        public List<StudentRequestV1> GetStudentsByNameEmail()
        {
            var students = _context.Students
                .Where(s => s.Active)
                .OrderByDescending(s => s.LastName)
                .Select(s => new StudentRequestV1
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                })
                .ToList();

            return students;
        }

        [HttpGet]
        public List<StudentRequestV2> GetStudentsByNameAndGrade()
        {
            var students = _context.Students
                .Where(s => s.Active) 
                .OrderByDescending(s => s.FirstName) 
                .Select(s => new StudentRequestV2
                {
                    FirstName = s.FirstName, 
                    GradeId = s.GradeId    
                })
                .ToList();

            return students;
        }

        [HttpPut("UpdateV1/{id}")]
        public IActionResult UpdateV1(int id, StudentRequestV1 studentRequestV1)
        {
            var existingStudent = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);

            if (existingStudent == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            existingStudent.FirstName = studentRequestV1.FirstName;
            existingStudent.LastName = studentRequestV1.LastName;
            existingStudent.Email = studentRequestV1.Email;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("UpdateV2/{id}")]
        public IActionResult UpdateV2(int id, StudentRequestV2 studentRequestV2)
        {
            var existingStudent = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);

            if (existingStudent == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            // Actualizar solo los campos FirstName y GradeId
            existingStudent.FirstName = studentRequestV2.FirstName;
            existingStudent.GradeId = studentRequestV2.GradeId;

            _context.SaveChanges();

            return NoContent(); 
        }




        [HttpPost]
        public ActionResult<Student> Create(Student student)
        {
            if (student == null)
            {
                return BadRequest("Estudiante inválido");
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = student.StudentID }, student);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, Student student)
        {
            if (id != student.StudentID)
            {
                return BadRequest("El ID del estudiante no coincide.");
            }

            var existingStudent = _context.Students.FirstOrDefault(s => s.StudentID == id);

            if (existingStudent == null)
            {
                return NotFound("Estudiante no encontrado.");
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Phone = student.Phone;
            existingStudent.Email = student.Email;
            existingStudent.Active = student.Active;
            existingStudent.GradeId = student.GradeId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        public IActionResult InsertStudentsByGrade(StudentRequestV4 request)
        {
            if (request.Students == null || request.Students.Count == 0)
            {
                return BadRequest("La lista de estudiantes está vacía.");
            }

            foreach (var student in request.Students)
            {
                student.GradeId = request.IdGrade; 
                _context.Students.Add(student); 
            }

            _context.SaveChanges(); 
            return CreatedAtAction(nameof(GetById), new { id = request.Students.First().StudentID }, request.Students);
        }





        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.StudentID == id && s.Active);
            if (student == null)
            {
                return NotFound("Estudiante no encontrado");
            }

            student.Active = false;
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}
