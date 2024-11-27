using lab12.Models;
using lab12.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public EnrollmentsController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Enrollment> GetAll()
        {
            return _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .ToList();
        }

        // Obtener una inscripción por ID
        [HttpGet("{id}")]
        public ActionResult<Enrollment> GetById(int id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefault(e => e.EnrollmentID == id);

            if (enrollment == null)
            {
                return NotFound("Inscripción no encontrada.");
            }

            return enrollment;
        }

        [HttpPost]
        public IActionResult EnrollStudent([FromBody] EnrrollmentRequestV1 request)
        {
            var enrollments = request.CourseIDs.Select(courseId => new Enrollment
            {
                StudentID = request.IdStudent,
                CourseID = courseId,
                Date = DateTime.Now
            }).ToList();

            _context.Enrollments.AddRange(enrollments);
            _context.SaveChanges();

            return Created("Matriculation completed", enrollments);
        }

        [HttpPost]
        public ActionResult<Enrollment> Create(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                return BadRequest("Inscripción inválida.");
            }

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = enrollment.EnrollmentID }, enrollment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return BadRequest("El ID de la inscripción no coincide.");
            }

            var existingEnrollment = _context.Enrollments.FirstOrDefault(e => e.EnrollmentID == id);
            if (existingEnrollment == null)
            {
                return NotFound("Inscripción no encontrada.");
            }

            existingEnrollment.Date = enrollment.Date;
            existingEnrollment.CourseID = enrollment.CourseID;
            existingEnrollment.StudentID = enrollment.StudentID;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.EnrollmentID == id);
            if (enrollment == null)
            {
                return NotFound("Inscripción no encontrada.");
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
