using System.ComponentModel.DataAnnotations;

namespace MainForm.Models {
    public class CompanyElementsModels {
        public int CompanyId {
            get; set;
        }

        [Required]
        public string CompanyName { get; set; } = string.Empty; // 初期値を設定

        [Required]
        public int Hourly {
            get; set;
        }

        [Required]
        public string WorkplaceAddress { get; set; } = string.Empty; // 初期値を設定

        [EmailAddress]
        public string Email { get; set; } = string.Empty; // 初期値を設定

        [Phone]
        public string Phone { get; set; } = string.Empty; // 初期値を設定

        [Required]
        public string Password { get; set; } = string.Empty; // 初期値を設定
    }
}
