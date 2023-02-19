using System.ComponentModel.DataAnnotations;

namespace PogodaArchive.Models
{
    public class YearMonthModel
    {
        public int Id { get; set; } // Id
        
        [Required(ErrorMessage = "Пожалуйста, выберите год просмотра")]
        public int Year { get; set; } // Год


        [Required(ErrorMessage = "Пожалуйста, выберите месяц просмотра")]
        public int Month { get; set; } // Месяц

        public string? Discription { get; set; } // Заголовок со страницы Excel выбранного месяца
    }
}
