using Microsoft.AspNetCore.Mvc;
using whm.DTOs.Department;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET ALL
        // GET: api/Department
        // =====================================================

        [HttpGet("Getall")]
        public async Task<IActionResult> GetAll()
        {
            var departments =
                await unitOfWork.Departments
                    .GetAllAsync();

            var result =
                departments.Select(d => new
                {
                    departmentId =
                        d.Department_Id,

                    departmentName =
                        d.Department_Name,

                    description =
                        d.Description,

                    isActive =
                        d.IsActive,

                    createAt =
                        d.CreateAt,

                    updateAt =
                        d.UpdateAt
                });

            return Ok(result);
        }


        // =====================================================
        // GET BY ID
        // GET: api/Department/1
        // =====================================================

        [HttpGet("Getbyid/{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            var department =
                await unitOfWork.Departments
                    .GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(
                    "Department not found.");
            }

            return Ok(new
            {
                departmentId =
                    department.Department_Id,

                departmentName =
                    department.Department_Name,

                description =
                    department.Description,

                isActive =
                    department.IsActive,

                createAt =
                    department.CreateAt,

                updateAt =
                    department.UpdateAt
            });
        }


        // =====================================================
        // GET BY NAME
        // GET: api/Department/GetbyName/{name}
        // =====================================================

        [HttpGet("GetbyName/{name}")]
        public async Task<IActionResult> GetByName(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(
                    "Department name is required.");
            }

            var department =
                await unitOfWork.Departments
                    .GetByNameAsync(name.Trim());

            if (department == null)
            {
                return NotFound(
                    "Department not found.");
            }

            return Ok(new
            {
                departmentId =
                    department.Department_Id,

                departmentName =
                    department.Department_Name,

                description =
                    department.Description,

                isActive =
                    department.IsActive,

                createAt =
                    department.CreateAt,

                updateAt =
                    department.UpdateAt
            });
        }


        // =====================================================
        // CREATE
        // POST: api/Department
        // =====================================================

        [HttpPost("Create")]
        public async Task<IActionResult> Create(
            CreateDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // =================================================
            // CHECK NAME
            // =================================================

            var existingDepartment =
                await unitOfWork.Departments
                    .GetByNameAsync(
                        dto.Department_Name.Trim());

            if (existingDepartment != null)
            {
                return BadRequest(
                    "Department already exists.");
            }


            // =================================================
            // CREATE
            // =================================================

            var department =
                new Department
                {
                    Department_Name =
                        dto.Department_Name.Trim(),

                    Description =
                        string.IsNullOrWhiteSpace(
                            dto.Description)
                            ? null
                            : dto.Description.Trim(),

                    IsActive =
                        true,

                    CreateAt =
                        DateTimeOffset.UtcNow,

                    UpdateAt =
                        DateTimeOffset.UtcNow
                };


            await unitOfWork.Departments
                .AddAsync(department);

            await unitOfWork
                .SaveAsync();


            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    id = department.Department_Id
                },
                new
                {
                    message =
                        "Department created successfully.",

                    departmentId =
                        department.Department_Id,

                    departmentName =
                        department.Department_Name,

                    description =
                        department.Description,

                    isActive =
                        department.IsActive,

                    createAt =
                        department.CreateAt,

                    updateAt =
                        department.UpdateAt
                });
        }


        // =====================================================
        // UPDATE
        // PUT: api/Department/1
        // =====================================================

        [HttpPut("Update{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateDepartmentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var department =
                await unitOfWork.Departments
                    .GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(
                    "Department not found.");
            }


            // =================================================
            // CHECK NAME
            // =================================================

            var existingDepartment =
                await unitOfWork.Departments
                    .GetByNameAsync(
                        dto.Department_Name.Trim());

            if (existingDepartment != null &&
                existingDepartment.Department_Id != id)
            {
                return BadRequest(
                    "Department already exists.");
            }


            // =================================================
            // UPDATE
            // =================================================

            department.Department_Name =
                dto.Department_Name.Trim();

            department.Description =
                string.IsNullOrWhiteSpace(
                    dto.Description)
                    ? null
                    : dto.Description.Trim();

            department.IsActive =
                dto.IsActive;

            department.UpdateAt =
                DateTimeOffset.UtcNow;


            unitOfWork.Departments
                .Update(department);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Department updated successfully.",

                departmentId =
                    department.Department_Id,

                departmentName =
                    department.Department_Name,

                description =
                    department.Description,

                isActive =
                    department.IsActive,

                updateAt =
                    department.UpdateAt
            });
        }


        // =====================================================
        // DELETE
        // DELETE: api/Department/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var department =
                await unitOfWork.Departments
                    .GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(
                    "Department not found.");
            }


            // =================================================
            // SOFT DELETE
            // =================================================

            department.IsActive =
                false;

            department.UpdateAt =
                DateTimeOffset.UtcNow;


            unitOfWork.Departments
                .Update(department);

            await unitOfWork
                .SaveAsync();


            return Ok(new
            {
                message =
                    "Department deleted successfully.",

                departmentId =
                    department.Department_Id,

                isActive =
                    department.IsActive,

                updateAt =
                    department.UpdateAt
            });
        }
    }
}