using AutoMapper;
using LeaveManagementSystem.Web.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Services;
using LeaveManagementSystem.Web.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
     [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesService leaveTypesService) : Controller
    {
        
        private const string SickLeaveExceededValidation = "The number of sick leave exceeded.";
        private const string LeaveTypeNameValidation = "The leave type name already exists.";


        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var viewData = await leaveTypesService.GetAllAsync();
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
           
            var leaveType = await leaveTypesService.GetByIdAsync<LeaveTypeReadOnlyVm>(id.Value);
            
            if (leaveType == null)
            {
                return NotFound();
            }
            
            return View(leaveType);
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
            if (await leaveTypesService.CheckIfNumberOfDaysExceeded(leaveTypeCreateVm.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", SickLeaveExceededValidation);
            } else if (await leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreateVm.Name))
            {
                ModelState.AddModelError("Name", LeaveTypeNameValidation);
            }

            if (!ModelState.IsValid)
            {
                await leaveTypesService.CreateAsync(leaveTypeCreateVm);
                return View(leaveTypeCreateVm);
            }
            
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
            var leaveType = await leaveTypesService.GetByIdAsync<LeaveTypeEditVm>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            
            return View(leaveType);
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
            
            if (await leaveTypesService.CheckIfNumberOfDaysExceeded(leaveTypeEdit.NumberOfDays))
            {
                ModelState.AddModelError("NumberOfDays", SickLeaveExceededValidation);
            } else if (await leaveTypesService.EditCheckIfLeaveTypeNameExists(leaveTypeEdit))
            {
                ModelState.AddModelError("Name", LeaveTypeNameValidation);
            }

            if (!ModelState.IsValid) return View(leaveTypeEdit);
            try
            {
                
                await leaveTypesService.EditAsync(leaveTypeEdit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
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

            var leaveType = await leaveTypesService.GetByIdAsync<LeaveTypeReadOnlyVm>(id.Value);
            if (leaveType == null)
            {
                return NotFound();
            }
            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await leaveTypesService.RemoveByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
