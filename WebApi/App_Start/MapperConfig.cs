using System;
using System.Collections.Generic;
using System.Text.Json;
using AutoMapper;
using WebApi.Models;
using WebApi.Projections;

namespace WebApi
{
    public class MapperConfig
    {
        public static void Configure(IMapperConfigurationExpression configAction)
        {
            if (configAction is null)
                throw new ArgumentNullException(nameof(configAction));

            configAction.CreateMap<FeedbackProjection, FeedbackModel>()
                .ForMember(destination => destination.Parameters,
                    x => x.MapFrom((source, _) => JsonSerializer.Deserialize<Dictionary<string, string>>(source.Parameters)));
            configAction.CreateMap<FeedbackModel, FeedbackProjection>()
                .ForMember(destination => destination.Parameters,
                    x => x.MapFrom((source, _) => JsonSerializer.Serialize(source.Parameters)));
        }
    }
}