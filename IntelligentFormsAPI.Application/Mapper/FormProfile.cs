﻿using AutoMapper;
using IntelligentFormsAPI.Application.Models;
using IntelligentFormsAPI.Application.Models.Form;
using IntelligentFormsAPI.Application.Models.FormTemplate;
using IntelligentFormsAPI.Domain.Entities;
using IntelligentFormsAPI.Domain.Enums;

namespace IntelligentFormsAPI.Application.Mapper
{

    public class FormProfile : Profile
    {
        public FormProfile()
        {
            CreateMap<FormDto, Form>();


            CreateMap<Form, FormCreateResponseDto>();

            CreateMap<SectionDto, Section>()
                .ForMember(dest => dest.DocumentType,
                opt => opt.MapFrom(
                    src => Enum.Parse<ScannableDocumentType>(src.DocumentType)));

            CreateMap<Section, SectionDto>()
                .ForMember(dest => dest.DocumentType,
                opt => opt.MapFrom(
                    src => src.DocumentType.ToString()));

        }
    }
}
