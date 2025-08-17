using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;

        // هكتب الكوفجريشن هنا عشان لو كتبتها فى الايدنتتى كوفجريشن هيوصلها لما يروح ينفذ ال فنكشن بتاع الريجستر انا بقى مش عاوزه يوصل اصلا لل فنكشنن لو غلط 
        [Required]
        // Password must contain 1 UpperCase, 1 Lowercase, 1 number, 1 non-alphanumeric character, and be at least 6 characters long
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number, 1 special character, and be at least 6 characters long.")]
        public string Password { get; set; } = null!; 
    }
}
