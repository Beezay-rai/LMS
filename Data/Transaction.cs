﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public virtual ICollection<BookTransaction> BookTransactions { get; set; }
    }

    public class BookTransaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public TransactionStatus Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public virtual Transaction Transaction { get; set; }

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
    }

    public enum TransactionStatus
    {
        Issued,
        Returned,
        Overdue
    }

    public enum TransactionType
    {
        Issue,
        Return,
        Fine
    }
}
