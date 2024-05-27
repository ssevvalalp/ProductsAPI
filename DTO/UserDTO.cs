﻿using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.DTO
{
    public class UserDTO
    {
        //kullanıcıya göndermek istenen alanlar

        [Required]
        public string FullName { get; set; } = null!;  
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
