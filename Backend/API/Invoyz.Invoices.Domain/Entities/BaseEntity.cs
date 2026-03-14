namespace Invoyz.Invoices.Domain.Entities
{
    public abstract class BaseEntity<TReadDto, TWriteDto> where TReadDto : class where TWriteDto : class
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public abstract TReadDto ToReadDto();
        public abstract void UpdateFromWriteDto(TWriteDto dto);
    }
}
