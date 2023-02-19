using System.ComponentModel.DataAnnotations;

namespace PogodaArchive.Models
{
    public class PogodaModel
    {
        public int Id { get; set; } // Id

        public int Number { get; set; } // Порядковый номер строки на листе

        public int Year { get; set; } // Год

        public int Month { get; set; } // Месяц

        [Display(Name = "Дата")]
        public string? Date { get; set; } // Дата

        [Display(Name = "Время, мск.")]
        public string? Time { get; set; } // Время

        [Display(Name = "Т")]
        public string? Temp { get; set; }  // Температура

        [Display(Name = "Влажность, %")]
        public string? Humidity { get; set; } // Влажность

        [Display(Name = "Td")]
        public string? DewPoint { get; set; } // Точка росы

        [Display(Name = "Атм. давление, мм рт.ст.")]
        public string? AtmPressure { get; set; } // Атмосферное давление

        [Display(Name = "Напр. ветра")]
        public string? WindDirection { get; set; } // Направление ветра

        [Display(Name = "Ск. ветра")]
        public string? WindSpeed { get; set; } // Скорость ветра

        [Display(Name = "Облачность")]
        public string? CloudCover { get; set; } // Облачность

        [Display(Name = "h")]
        public string? LowerCloudLimit { get; set; } // Нижняя граница облачности

        [Display(Name = "VV")]
        public string? HorVisibility { get; set; } // Горизонтальная видимость

        [Display(Name = "Погодные явления")]
        public string? WeatherEvents { get; set; } // Погодные явления
    }
}
