using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const string SickLeaveExceededValidation = "The number of sick leave exceeded.";
        private const string LeaveTypeNameValidation = "The leave type name already exists.";

        public LeaveTypesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            // SELECT * FROM LeaveTypes
            var data = await _context.LeaveTypes.ToListAsync();
            // convert the datamodel into a view model - use AutoMapper
            var viewData = _mapper.Map<List<LeaveTypeReadOnlyVm>>(data);
            // return the view model to the view
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Parameterization - key for preventing SQL Injection attacks
            // SELECT * FROM LeaveTypes WHERE Id = @id
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (leaveType == null)
            {
                return NotFound();
            }
            
            var viewData = _mapper.Map<LeaveTypeReadOnlyVm>(leaveType);

            return View(viewData);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVm leaveTypeCreateVm)
        {
            if (await CheckIfNumberOfDaysExceeded(leaveTypeCreateVm.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", SickLeaveExceededValidation);
            } else if (await CheckIfLeaveTypeNameExists(leaveTypeCreateVm.Name))
            {
                ModelState.AddModelError("Name", LeaveTypeNameValidation);
            }

            if (!ModelState.IsValid) return View(leaveTypeCreateVm);
            var leaveType = _mapper.Map<LeaveType>(leaveTypeCreateVm);
            _context.Add(leaveType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // SELECT * FROM LeaveTypes WHERE Id = @id
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }
            
            var viewData = _mapper.Map<LeaveTypeEditVm>(leaveType);
            return View(viewData);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveTypeEditVm leaveTypeEdit)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }
            
            
            
            if (await CheckIfNumberOfDaysExceeded(leaveTypeEdit.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", SickLeaveExceededValidation);
            } else if (await CheckIfLeaveTypeNameExists(leaveTypeEdit.Name))
            {
                ModelState.AddModelError("Name", LeaveTypeNameValidation);
            }

            if (!ModelState.IsValid) return View(leaveTypeEdit);
            try
            {
                var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                _context.Update(leaveType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(leaveTypeEdit.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var viewData = _mapper.Map<LeaveTypeReadOnlyVm>(leaveType);
            return View(viewData);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }

        private async Task<bool> CheckIfLeaveTypeNameExists(string name)
        {
            var lowerCaseName = name.ToLower();
            return await _context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(lowerCaseName));
        }

        private async Task<bool> EditCheckIfLeaveTypeNameExists(LeaveTypeEditVm leaveTypeEdit)
        {
            var lowerCaseName = leaveTypeEdit.Name.ToLower();
            return await _context.LeaveTypes.AnyAsync(e => e.Name.ToLower().Equals(lowerCaseName) && e.Id != leaveTypeEdit.Id);
        }
        private async Task<bool> CheckIfNumberOfDaysExceeded(decimal numberOfDays)
        {
            return await _context.LeaveTypes.AnyAsync(t => t.NumberOfDays > 24.0m);
        }
    }
}
