using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using JokesApiContracts.Domain.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JokesApi.Impl.Services.EntityFramework
{
    public class DbJokesService : IJokesService
    {
        private readonly MapperConfiguration mappingConfig;

        private readonly IServiceProvider serviceProvider;

        public DbJokesService(
            IServiceProvider serviceProvider)
        {
            this.mappingConfig = new MapperConfiguration(config => config.CreateMap<DbJokeEntity, JokeModel>());
            this.mappingConfig = new MapperConfiguration(config => config.CreateMap<JokeImportModel, DbJokeEntity>());

            this.serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<JokesLanguageModel>> GetLanguagesAsync(
            CancellationToken cancellationToken)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var jokesContext = scope.ServiceProvider.GetService<DbJokesContext>();

                var query = jokesContext.Jokes.AsNoTracking();
                var languages = await query.GroupBy(joke => joke.Language)
                   .Select(
                        byLanguage =>
                            new JokesLanguageModel
                            {
                                Name = byLanguage.Key,
                                Categories = byLanguage.GroupBy(joke => joke.Category)
                                   .Select(
                                        byCategory => new JokesLanguageCategoryModel
                                        {
                                            Name = byCategory.Key,
                                            Count = byCategory.Count()
                                        })
                                   .ToArray()
                            })
                   .ToArrayAsync(cancellationToken);

                return languages;
            }
        }

        public async Task<IEnumerable<JokeModel>> GetJokesAsync(
            string language,
            string category,
            CancellationToken cancellationToken)
        {
            var mapper = this.mappingConfig.CreateMapper();

            using (var scope = this.serviceProvider.CreateScope())
            {
                var jokesContext = scope.ServiceProvider.GetService<DbJokesContext>();

                var query = jokesContext.Jokes.AsNoTracking();

                if (!string.IsNullOrEmpty(language))
                {
                    query = query.Where(joke => joke.Language == language);
                }

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(joke => joke.Category == category);
                }

                var jokes = await query.ToArrayAsync(cancellationToken);

                return mapper.Map<IEnumerable<DbJokeEntity>, IEnumerable<JokeModel>>(jokes);
            }
        }

        public async Task ImportJokesAsync(
            IEnumerable<JokeImportModel> jokes,
            CancellationToken cancellationToken)
        {
            var mapper = this.mappingConfig.CreateMapper();

            using (var scope = this.serviceProvider.CreateScope())
            {
                var jokesContext = scope.ServiceProvider.GetService<DbJokesContext>();

                foreach (var entity in mapper.Map<IEnumerable<JokeImportModel>, IEnumerable<DbJokeEntity>>(jokes))
                {
                    var existing = jokesContext.Jokes.Find(entity.Id);
                    if (existing == null)
                    {
                        jokesContext.Jokes.Add(entity);
                    }
                    else
                    {
                        existing.Name = entity.Name;
                        existing.Author = entity.Author;
                        existing.Language = entity.Language;
                        existing.Category = entity.Category;
                        existing.PublishDate = entity.PublishDate;
                    }
                }

                await jokesContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}