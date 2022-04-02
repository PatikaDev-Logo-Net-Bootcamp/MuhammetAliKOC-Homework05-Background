using Homework05_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Homework05_DataAccess.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            //Veritabanı oluşturulurken test için yada başka amaçlarla otomatik veri oluşturulmasını istiyorsak aşağıdaki kod kullanılabilir. SeetUsers() List<User> dönen bir metod olmalı!
            //builder.HasData(SeedUsers());
        }
    }
}
