using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PogodaArchive.Data;
using PogodaArchive.Models;

namespace PogodaArchive.Controllers
{
    public class PogodaModelsController : Controller
    {
        private readonly PogodaArchiveContext _context;

        public PogodaModelsController(PogodaArchiveContext context)
        {
            _context = context;
        }


        // GET: PogodaModels
        public IActionResult Index(int pg, int year, int month)
        {
            // Проверка есть ли данные в базе. Если есть, то заполняем выпадающие списки год и месяц для просмотра архива.
            if (_context.YearMonthModel.Count() != 0)
            {
                var yearsList = _context.YearMonthModel.OrderByDescending(x => x.Year)
                    .Select(x => x.Year)
                    .Distinct()
                    .Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() });

                ViewBag.Years = yearsList;


                ViewBag.Month = new List<SelectListItem>
                    {
                        new SelectListItem {Text = "Январь", Value = "1"},
                        new SelectListItem {Text = "Февраль", Value = "2"},
                        new SelectListItem {Text = "Март", Value = "3"},
                        new SelectListItem {Text = "Апрель", Value = "4"},
                        new SelectListItem {Text = "Май", Value = "5"},
                        new SelectListItem {Text = "Июнь", Value = "6"},
                        new SelectListItem {Text = "Июль", Value = "7"},
                        new SelectListItem {Text = "Август", Value = "8"},
                        new SelectListItem {Text = "Сентябрь", Value = "9"},
                        new SelectListItem {Text = "Октябрь", Value = "10"},
                        new SelectListItem {Text = "Ноябрь", Value = "11"},
                        new SelectListItem {Text = "Декабрь", Value = "12"}

                    };

            }

            // При нажатии на вкладку меню Просмотр срабатывает данный контроллер (Index) при этом год и месяц нули.
            // При выборе постраничного просмотра также срабатывает данный контроллер, но при этом в него передаются данные о странице, годе и месяце просмотра.
            // Т.о. ниже осуществляется проверка было ли это открытие вкладки Просмотр или постраничный просмотр.
            // Если это был постраничный просмотр, то срабатывает логика постраничного просмотра.
            if (year != 0 & month != 0)
            {
                var ymModel = new YearMonthModel();
                ymModel.Year = year;
                ymModel.Month = month;
                ViewBag.DiscList = _context.YearMonthModel.Where(x => x.Year == year & x.Month == month).Select(y => y.Discription).First();

                ViewBag.YMModel = ymModel;



                var pogodaList = _context.PogodaModel.Where(x => x.Year == year & x.Month == month).OrderBy(x => x.Number).ToList();

                const int pageSize = 10;

                if (pg < 1) pg = 1;

                int recsCount = pogodaList.Count();

                var pager = new Pager(recsCount, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = pogodaList.Skip(recSkip).Take(pager.PageSize).ToList();

                ViewBag.Pager = pager;

                return View(data);
            }

            return View();
        }

        // Контроллер просмотра выбранного года и месяца
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int year, int month)
        {

            if (ModelState.IsValid)
            {
                // Заполнение выпадающих списков год и месяц, а также заголовка таблицы просмотра
                var yearsList = _context.YearMonthModel.OrderByDescending(x => x.Year)
                    .Select(x => x.Year)
                    .Distinct()
                    .Select(x => new SelectListItem() { Text = x.ToString(), Value = x.ToString() });

                ViewBag.Years = yearsList;

                var pogodaList = await _context.PogodaModel.Where(x => x.Year == year & x.Month == month).OrderBy(x => x.Number).ToListAsync();


                ViewBag.DiscList = _context.YearMonthModel.Where(x => x.Year == year & x.Month == month).Select(y => y.Discription).First();

                ViewBag.Month = new List<SelectListItem>
                    {
                        new SelectListItem {Text = "Январь", Value = "1"},
                        new SelectListItem {Text = "Февраль", Value = "2"},
                        new SelectListItem {Text = "Март", Value = "3"},
                        new SelectListItem {Text = "Апрель", Value = "4"},
                        new SelectListItem {Text = "Май", Value = "5"},
                        new SelectListItem {Text = "Июнь", Value = "6"},
                        new SelectListItem {Text = "Июль", Value = "7"},
                        new SelectListItem {Text = "Август", Value = "8"},
                        new SelectListItem {Text = "Сентябрь", Value = "9"},
                        new SelectListItem {Text = "Октябрь", Value = "10"},
                        new SelectListItem {Text = "Ноябрь", Value = "11"},
                        new SelectListItem {Text = "Декабрь", Value = "12"}

                    };

                // Логика постраничного просмотра
                const int pageSize = 10;

                var pg = 1;

                int recsCount = pogodaList.Count();

                var pager = new Pager(recsCount, pg, pageSize);

                int recSkip = (pg - 1) * pageSize;

                var data = pogodaList.Skip(recSkip).Take(pager.PageSize).ToList();

                ViewBag.Pager = pager;

                return View(data);

            }

            return View();
        }

        // Контроллер открытия формы заполнения БД
        public IActionResult LoadData()
        {
            return View();
        }

        // Контроллер заполнения БД выбранными файлами
        [HttpPost]
        public IActionResult LoadData(List<IFormFile> postedFiles)
        {
            if (postedFiles.Count() == 0)
            {
                ViewBag.Error += "Пожалуйста, выберете файлы для загрузки";
                return View();
            }

            var fileName = "";

            try
            {
                foreach (var file in postedFiles)
                {
                    fileName = file.FileName;

                    var listYearMonth = new List<YearMonthModel>();
                    var listPogoda = new List<PogodaModel>();

                    // Блок чтения данных из выбранного файла. Используется библиотека NPOI.
                    using (Stream stream = file.OpenReadStream())
                    {
                        IWorkbook MyExcel = null;

                        if (Path.GetExtension(file.FileName) == ".xlsx")
                        {
                            MyExcel = new XSSFWorkbook(stream);
                        }
                        else if (Path.GetExtension(file.FileName) == ".xls")
                        {
                            MyExcel = new HSSFWorkbook(stream);
                        }

                        else
                        {
                            ViewBag.Error += "Неверный формат файла!";
                            return View();
                        }

                        // выбор страницы файла Excel (12 месяцев)
                        for (int i = 0; i < 12; i++) 
                        {
                            ISheet sheet = MyExcel.GetSheetAt(i);

                            if (sheet != null)
                            {
                                // номер последнего ряда на странице
                                int lastRowNum = sheet.LastRowNum;  

                                IRow endRow = sheet.GetRow(lastRowNum);

                                // берем первую ячейку последнего ряда (дата) и если ячейка не пустая идем далее
                                var date = endRow.GetCell(0)?.ToString(); 

                                if (date != null)
                                {
                                    // разделяем строку находим месяц и год
                                    string[] words = date.Split('.');

                                    int year = int.Parse(words[2]);
                                    int month = 0;

                                    switch (words[1])
                                    {
                                        case "01":
                                            month = 1;
                                            break;
                                        case "02":
                                            month = 2;
                                            break;
                                        case "03":
                                            month = 3;
                                            break;
                                        case "04":
                                            month = 4;
                                            break;
                                        case "05":
                                            month = 5;
                                            break;
                                        case "06":
                                            month = 6;
                                            break;
                                        case "07":
                                            month = 7;
                                            break;
                                        case "08":
                                            month = 8;
                                            break;
                                        case "09":
                                            month = 9;
                                            break;
                                        case "10":
                                            month = 10;
                                            break;
                                        case "11":
                                            month = 11;
                                            break;
                                        case "12":
                                            month = 12;
                                            break;
                                    }

                                    // проверяем есть ли такой месяц и год в базе
                                    var yearMonthModel = _context.YearMonthModel.Where(x => x.Year == year & x.Month == month).ToList();

                                    if (yearMonthModel.Count == 0 & month != 0)
                                    {
                                        // взять заголовок страницы
                                        var strBilder = new StringBuilder();
                                        var description = strBilder.Append(sheet.GetRow(0).GetCell(0).ToString())
                                                                   .Append(' ')
                                                                   .Append(sheet.GetRow(1).GetCell(0).ToString()).ToString();

                                        // добавить в лист объект YearMonthModel
                                        listYearMonth.Add(new YearMonthModel
                                        {
                                            Year = year,
                                            Month = month,
                                            Discription = description
                                        });

                                        for (int j = 4; j <= lastRowNum; j++)
                                        {

                                            IRow currentRow = sheet.GetRow(j);

                                            // добавить в лист объект PogodaModel
                                            listPogoda.Add(new PogodaModel
                                            {
                                                Number = j,
                                                Year = year,
                                                Month = month,
                                                Date = currentRow.GetCell(0)?.ToString(),
                                                Time = currentRow.GetCell(1)?.ToString(),
                                                Temp = currentRow.GetCell(2)?.ToString(),
                                                Humidity = currentRow.GetCell(3)?.ToString(),
                                                DewPoint = currentRow.GetCell(4)?.ToString(),
                                                AtmPressure = currentRow.GetCell(5)?.ToString(),
                                                WindDirection = currentRow.GetCell(6)?.ToString(),
                                                WindSpeed = currentRow.GetCell(7)?.ToString(),
                                                CloudCover = currentRow.GetCell(8)?.ToString(),
                                                LowerCloudLimit = currentRow.GetCell(9)?.ToString(),
                                                HorVisibility = currentRow.GetCell(10)?.ToString(),
                                                WeatherEvents = currentRow.GetCell(12)?.ToString(),
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Сохранение прочитанных данных в БД
                    _context.BulkInsert(listYearMonth);
                    _context.BulkInsert(listPogoda);

                    ViewBag.Message += fileName + ",";

                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ViewBag.Error += fileName + " - " + ex.InnerException.Message;
                    return View();
                }
                else if (ex.Message != null)
                {
                    ViewBag.Error += fileName + " - " + ex.Message;
                    return View();
                }
                else
                {
                    ViewBag.Error += fileName + " - " + "Ошибка загрузки данных!";
                    return View();
                }
            }

            return View();
        }


        // GET: PogodaModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PogodaModel == null)
            {
                return NotFound();
            }

            var pogodaModel = await _context.PogodaModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pogodaModel == null)
            {
                return NotFound();
            }

            return View(pogodaModel);
        }

        // GET: PogodaModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PogodaModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Year,Month,Date,Time,Temp,Humidity,DewPoint,AtmPressure,WindDirection,WindSpeed,CloudCover,LowerCloudLimit,HorVisibility,WeatherEvents")] PogodaModel pogodaModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pogodaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pogodaModel);
        }

        // GET: PogodaModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PogodaModel == null)
            {
                return NotFound();
            }

            var pogodaModel = await _context.PogodaModel.FindAsync(id);
            if (pogodaModel == null)
            {
                return NotFound();
            }
            return View(pogodaModel);
        }

        // POST: PogodaModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Year,Month,Date,Time,Temp,Humidity,DewPoint,AtmPressure,WindDirection,WindSpeed,CloudCover,LowerCloudLimit,HorVisibility,WeatherEvents")] PogodaModel pogodaModel)
        {
            if (id != pogodaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pogodaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PogodaModelExists(pogodaModel.Id))
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
            return View(pogodaModel);
        }

        // GET: PogodaModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PogodaModel == null)
            {
                return NotFound();
            }

            var pogodaModel = await _context.PogodaModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pogodaModel == null)
            {
                return NotFound();
            }

            return View(pogodaModel);
        }

        // POST: PogodaModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PogodaModel == null)
            {
                return Problem("Entity set 'PogodaArchiveContext.PogodaModel'  is null.");
            }
            var pogodaModel = await _context.PogodaModel.FindAsync(id);
            if (pogodaModel != null)
            {
                _context.PogodaModel.Remove(pogodaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PogodaModelExists(int id)
        {
            return _context.PogodaModel.Any(e => e.Id == id);
        }
    }
}
