using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Features
{
    public static class MappingExtensions
    {
        public static ICollectionMapperExpression Map<TSource>(this IEnumerable<TSource> items)
            => new CollectionMapperExpression<TSource>(items);

        public static IQueryableExpression Map<TSource>(this IQueryable<TSource> items)
            => new QueryableExpression<TSource>(items);

        private class CollectionMapperExpression<TSource> : ICollectionMapperExpression
        {
            private readonly IEnumerable<TSource> _items;

            public CollectionMapperExpression(IEnumerable<TSource> items)
            {
                _items = items;
            }

            public IList<TDestination> ToList<TDestination>() => _items.Select(Mapper.Map<TSource, TDestination>).ToList();
        }

        private class QueryableExpression<TSource> : IQueryableExpression
        {
            private readonly IQueryable<TSource> _items;

            public QueryableExpression(IQueryable<TSource> items)
            {
                _items = items;
            }


            public async Task<IList<TDestination>> ToListAsync<TDestination>()
            {
                var returned = await _items.ToListAsync();

                return returned.Select(src => Mapper.Map<TSource, TDestination>(src)).ToList();
            }
        }
    }

    public interface ICollectionMapperExpression
    {
        IList<TDestination> ToList<TDestination>();
    }

    public interface IQueryableExpression
    {
        Task<IList<TDestination>> ToListAsync<TDestination>();
    }
}