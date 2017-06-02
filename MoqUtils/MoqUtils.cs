using Moq;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MoqUtils
{
    public static class MoqUtils
    {
        public static Mock<DbSet<T>> GetDbSetMock<T>(IQueryable<T> data) where T:class{

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new DbAsyncEnumerator<T>(data.GetEnumerator()));

            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new DbAsyncQueryProvider<T>(data.Provider));

            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockSet.Setup(x => x.AsNoTracking()).Returns(mockSet.Object);
            mockSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockSet.Object);


            return mockSet;
        }
    }
}
