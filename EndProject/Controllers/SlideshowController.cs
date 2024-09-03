using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EndProject.Models;

public class SlideshowController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public SlideshowController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(UploadImageViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Ensure the images are saved in the slideshow folder
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath,"images" ,"Slideshow");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                return RedirectToAction(nameof(Manage));
            }
        }

        return View(model);
    }

    public IActionResult Manage()
    {
        // Get images from the slideshow folder
        var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath,"images","Slideshow");
        var imagePaths = Directory.GetFiles(imageFolder)
                                  .Select(f => "/images/Slideshow/" + Path.GetFileName(f))
                                  .ToList();

        return View(imagePaths);
    }

    [HttpPost]
    public IActionResult Delete(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return NotFound();
        }

        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath,"images","Slideshow", fileName);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }

        return RedirectToAction(nameof(Manage));
    }
}
