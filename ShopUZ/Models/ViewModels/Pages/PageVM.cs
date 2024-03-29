﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ShopUZ.Models.Data;

namespace ShopUZ.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        {

        }

        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }


        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Tytuł strony")]
        public string Title { get; set; }[StringLength(50, MinimumLength = 3)]
        [Display(Name = "Adres strony")]
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [Display(Name = "Zawartość strony")]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        [Display(Name = "Pasek boczny")]
        public bool HasSidebar { get; set; }
    }
}