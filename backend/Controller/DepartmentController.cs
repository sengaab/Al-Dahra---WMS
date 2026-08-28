using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using whm.DTOs;
using whm.Models;
using whm.UnitOfWork;

namespace whm.Controllers
{
    [ApiController]
    [Route("api/departments")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // =====================================================
        // GET: api/departments
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDTO>>>
            GetDepartments()
        {
            var departments =
                await _unitOfWork.Department.GetAllAsync();

            var response = departments
                .Select(d => new DepartmentResponseDTO
                {
                    DepartmentId = d.DepartmentId,
                    Name = d.Name,
                   
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    UsersCount = d.Users?.Count ?? 0
                })
                .ToList();

            return Ok(response);
        }


        // =====================================================
        // GET: api/departments/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentResponseDTO>>
            GetDepartment(int id)
        {
            var department =
                await _unitOfWork.Department.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }

            var response = new DepartmentResponseDTO
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
               
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
                UsersCount = department.Users?.Count ?? 0
            };

            return Ok(response);
        }


        // =====================================================
        // POST: api/departments
        // =====================================================

        [HttpPost]
        public async Task<ActionResult<DepartmentResponseDTO>>
            CreateDepartment(
                [FromBody] CreateDepartmentDTO dto)
        {
            // -------------------------------------------------
            // Check Name
            // -------------------------------------------------

            var nameExists =
                await _unitOfWork.Department
                    .NameExistsAsync(dto.Name);

            if (nameExists)
            {
                return Conflict(new
                {
                    message =
                        "A department with this name already exists."
                });
            }


            // -------------------------------------------------
            // Check Code
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var codeExists =
                    await _unitOfWork.Department
                        .CodeExistsAsync(dto.Code);

                if (codeExists)
                {
                    return Conflict(new
                    {
                        message =
                            "A department with this code already exists."
                    });
                }
            }


            // -------------------------------------------------
            // Create
            // -------------------------------------------------

            var department = new Department
            {
                Name = dto.Name.Trim(),
               

                IsActive = dto.IsActive,

                CreatedAt = DateTimeOffset.UtcNow,

                UpdatedAt = DateTimeOffset.UtcNow
            };


            await _unitOfWork.Department
                .AddAsync(department);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Response
            // -------------------------------------------------

            var response = new DepartmentResponseDTO
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
               
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
                UsersCount = 0,
                RequestsCount = 0
            };


            return CreatedAtAction(
                nameof(GetDepartment),
                new { id = department.DepartmentId },
                response);
        }


        // =====================================================
        // PUT: api/departments/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DepartmentResponseDTO>>
            UpdateDepartment(
                int id,
                [FromBody] UpdateDepartmentDTO dto)
        {
            // -------------------------------------------------
            // Get Department
            // -------------------------------------------------

            var department =
                await _unitOfWork.Department.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }


            // -------------------------------------------------
            // Check Name
            // -------------------------------------------------

            var nameExists =
                await _unitOfWork.Department
                    .NameExistsAsync(
                        dto.Name,
                        id);

            if (nameExists)
            {
                return Conflict(new
                {
                    message =
                        "A department with this name already exists."
                });
            }


            // -------------------------------------------------
            // Check Code
            // -------------------------------------------------

            if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var codeExists =
                    await _unitOfWork.Department
                        .CodeExistsAsync(
                            dto.Code,
                            id);

                if (codeExists)
                {
                    return Conflict(new
                    {
                        message =
                            "A department with this code already exists."
                    });
                }
            }


            // -------------------------------------------------
            // Update
            // -------------------------------------------------

            department.Name = dto.Name.Trim();
    

            department.IsActive = dto.IsActive;

            department.UpdatedAt =
                DateTimeOffset.UtcNow;


            _unitOfWork.Department
                .Update(department);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            // -------------------------------------------------
            // Reload
            // -------------------------------------------------

            var updatedDepartment =
                await _unitOfWork.Department
                    .GetByIdAsync(id);

            if (updatedDepartment == null)
            {
                return StatusCode(500, new
                {
                    message =
                        "Department was updated but could not be retrieved."
                });
            }


            var response = new DepartmentResponseDTO
            {
                DepartmentId =
                    updatedDepartment.DepartmentId,

                Name =
                    updatedDepartment.Name,


                IsActive =
                    updatedDepartment.IsActive,

                CreatedAt =
                    updatedDepartment.CreatedAt,

                UpdatedAt =
                    updatedDepartment.UpdatedAt,

                UsersCount =
                    updatedDepartment.Users?.Count ?? 0
            };


            return Ok(response);
        }


        // =====================================================
        // DELETE: api/departments/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult>
            DeleteDepartment(int id)
        {
            // -------------------------------------------------
            // Get Department
            // -------------------------------------------------

            var department =
                await _unitOfWork.Department.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }


            // -------------------------------------------------
            // Delete
            // -------------------------------------------------

            _unitOfWork.Department
                .Delete(department);


            // -------------------------------------------------
            // SAVE ASYNC
            // -------------------------------------------------

            await _unitOfWork.SaveAsync();


            return Ok(new
            {
                message =
                    "Department deleted successfully.",

                departmentId = id
            });
        }


        // =====================================================
        // GET:
        // api/departments/{id}/users
        // =====================================================

        [HttpGet("{id:int}/users")]
        public async Task<ActionResult<IEnumerable<DepartmentUserDTO>>>
            GetDepartmentUsers(int id)
        {
            // -------------------------------------------------
            // Check Department
            // -------------------------------------------------

            var department =
                await _unitOfWork.Department
                    .GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }


            // -------------------------------------------------
            // Get Users
            // -------------------------------------------------

            var users =
                await _unitOfWork.Department
                    .GetUsersAsync(id);


            var response = users
                .Select(u => new DepartmentUserDTO
                {
                    UserId = u.UserId,

                    EmployeeCode =
                        u.EmployeeCode,

                    Name =
                        u.Name,

                    Email =
                        u.Email,

                    RoleId =
                        u.RoleId,

                    RoleName =
                        u.Role?.Name,

                    IsActive =
                        u.IsActive
                })
                .ToList();


            return Ok(response);
        }


        // =====================================================
        // GET:
        // api/departments/{id}/requests
        // =====================================================

        [HttpGet("{id:int}/requests")]
        public async Task<ActionResult<IEnumerable<DepartmentRequestDTO>>>
            GetDepartmentRequests(int id)
        {
            // -------------------------------------------------
            // Check Department
            // -------------------------------------------------

            var department =
                await _unitOfWork.Department
                    .GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(new
                {
                    message = "Department not found."
                });
            }


            // -------------------------------------------------
            // Get Requests
            // -------------------------------------------------

            var requests =
                await _unitOfWork.Department
                    .GetRequestsAsync(id);


            var response = requests
                .Select(r => new DepartmentRequestDTO
                {
                    RequestId = r.RequestId,

                    RequestNumber =
                        r.RequestNumber,

                    RequestedBy =
                        r.RequestedBy,

                    RequesterName =
                        r.Requester?.Name,

                    DepartmentId =
                        r.DepartmentId,

                    Status =
                        r.StockRequestStatus.ToString(),

                })
                .ToList();


            return Ok(response);
        }
    }
}
