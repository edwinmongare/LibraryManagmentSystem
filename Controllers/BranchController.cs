using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Branch;
using LibraryData;
using Microsoft.AspNetCore.Mvc;


    public class BranchController : Controller
{
    private  ILibraryBranch _branch;

    public BranchController(ILibraryBranch branch)
    {
        _branch = branch;
    }

    public IActionResult Index()
    {
        var branchModels = _branch.GetAll()
            .Select(branch => new BranchDetailModel
            {
                Id = branch.Id,
                Name = branch.Name,
                IsOpen = _branch.IsBranchOpen(branch.Id),
                NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
                NumberOfPatrons = _branch.GetPatrons(branch.Id).Count(),

            });

        var model = new BranchIndexModel
        {
            Branches = branchModels
        };

        return View(model);
    }

    public IActionResult Detail(int id)
    {
        var branch = _branch.Get(id);
        var model = new BranchDetailModel
        {
            Name = branch.Name,
            Description = branch.Description,
            Address = branch.Address,
            Telephone = branch.Telephone,
            OpenDate = branch.OpenDate.ToString("yyyy-MM-dd"),
            NumberOfPatrons = _branch.GetPatrons(id).Count(),
            NumberOfAssets = _branch.GetAssets(branch.Id).Count(),
            TotalAssetValue = _branch.GetAssets(id).Sum(a =>a.Cost),
            ImageUrl = branch.ImageUrl,
            HoursOpen = _branch.GetBranchHours(id)
        };

        return View(model);
    }

}