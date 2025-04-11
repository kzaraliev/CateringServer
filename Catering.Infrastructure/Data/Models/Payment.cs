using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Catering.Infrastructure.Constants.PaymentConstants;

namespace Catering.Infrastructure.Data.Models
{
    /// <summary>
    /// Represents a payment for an order in the catering system.
    /// </summary>
    [Comment("Represents a payment for an order in the catering system.")]
    public class Payment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the payment.
        /// </summary>
        [Key]
        [Comment("Payment Identifier")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the order this payment is for.
        /// </summary>
        [Required]
        [Comment("Order Identifier")]
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the order this payment is for.
        /// </summary>
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Gets or sets the amount of the payment.
        /// </summary>
        [Required]
        [Comment("Payment amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the payment method used.
        /// </summary>
        [Required]
        [Comment("Payment method used (Card, BankTransfer, or OnDelivery)")]
        public required PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the status of the payment.
        /// </summary>
        [Required]
        [Comment("Payment status")]
        public required PaymentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier from the payment processor.
        /// </summary>
        [MaxLength(TransactionIdMaxLength)]
        [Comment("Transaction identifier from the payment processor")]
        public string? TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the payment was created.
        /// </summary>
        [Required]
        [Comment("Date and time when the payment was created")]
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the payment was processed.
        /// </summary>
        [Comment("Date and time when the payment was processed")]
        public DateTime? ProcessedAt { get; set; }
    }

    /// <summary>
    /// Represents the possible payment methods.
    /// </summary>
    public enum PaymentMethod
    {
        Card = 0,
        BankTransfer = 1,
        OnDelivery = 2
    }

    /// <summary>
    /// Represents the possible payment statuses.
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4,
        Cancelled = 5
    }
} 