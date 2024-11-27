using lab12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab12.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly InvoiceContext _context;

        public GradesController(InvoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Grade> GetAll()
        {
            return _context.Grades.Where(g => g.Active).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Grade> GetById(int id)
        {
            var grade = _context.Grades.FirstOrDefault(g => g.GradeID == id && g.Active);
            if (grade == null)
            {
                return NotFound(); 
            }
            return grade;
        }

        [HttpPost]
        public ActionResult<Grade> Create(Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grado inválido");
            }

            _context.Grades.Add(grade);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = grade.GradeID }, grade);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, Grade grade)
        {
            if (id != grade.GradeID)
            {
                return BadRequest("El ID del grado no coincide");
            }

            var existingGrade = _context.Grades.FirstOrDefault(g => g.GradeID == id && g.Active);
            if (existingGrade == null)
            {
                return NotFound("Grado no encontrado");
            }


            existingGrade.Name = grade.Name;
            existingGrade.Description = grade.Description;

            _context.SaveChanges();

            return NoContent(); 
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var grade = _context.Grades.FirstOrDefault(g => g.GradeID == id && g.Active);
            if (grade == null)
            {
                return NotFound("Grado no encontrado");
            }


            grade.Active = false;
            _context.SaveChanges();

            return NoContent(); 
        }
    }
}
