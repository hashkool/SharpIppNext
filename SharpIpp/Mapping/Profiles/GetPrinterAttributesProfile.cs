﻿using System;
using System.Collections.Generic;
using System.Linq;

using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Extensions;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Mapping.Profiles
{
    // ReSharper disable once UnusedMember.Global
    internal class GetPrinterAttributesProfile : IProfile
    {
        public void CreateMaps(IMapperConstructor mapper)
        {
            mapper.CreateMap<GetPrinterAttributesRequest, IppRequestMessage>((src, map) =>
            {
                var dst = new IppRequestMessage { IppOperation = IppOperation.GetPrinterAttributes };
                map.Map<IIppPrinterRequest, IppRequestMessage>(src, dst);
                var operation = dst.OperationAttributes;

                if (src.RequestedAttributes != null)
                {
                    operation.AddRange(src.RequestedAttributes.Select(requestedAttribute =>
                        new IppAttribute(Tag.Keyword, JobAttribute.RequestedAttributes, requestedAttribute)));
                }

                dst.OperationAttributes.Populate(src.AdditionalOperationAttributes);
                dst.JobAttributes.Populate(src.AdditionalJobAttributes);
                return dst;
            });

            mapper.CreateMap<IIppRequestMessage, GetPrinterAttributesRequest>( ( src, map ) =>
            {
                var dst = new GetPrinterAttributesRequest();
                map.Map<IIppRequestMessage, IIppPrinterRequest>( src, dst );
                var requestedAttributes = src.OperationAttributes.Where( x => x.Name == JobAttribute.RequestedAttributes ).Select( x => x.Value ).OfType<string>().ToArray();
                if ( requestedAttributes.Any() )
                    dst.RequestedAttributes = requestedAttributes;
                var knownOperationAttributeNames = new List<string> { JobAttribute.RequestedAttributes };
                dst.AdditionalOperationAttributes = src.OperationAttributes.Where( x => !knownOperationAttributeNames.Contains( x.Name ) ).ToList();
                dst.AdditionalJobAttributes = src.JobAttributes;
                return dst;
            } );

            mapper.CreateMap<IppResponseMessage, GetPrinterAttributesResponse>((src, map) =>
            {
                var dst = map.Map<GetPrinterAttributesResponse>(src.AllAttributes());
                map.Map<IppResponseMessage, IIppResponseMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<GetPrinterAttributesResponse, IppResponseMessage>( ( src, map ) =>
            {
                var dst = new IppResponseMessage();
                map.Map<IIppResponseMessage, IppResponseMessage>( src, dst );
                var section = new IppSection { Tag = SectionTag.PrinterAttributesTag };
                section.Attributes.AddRange( map.Map<IDictionary<string, IppAttribute[]>>( src ).Values.SelectMany( x => x ) );
                dst.Sections.Add( section );
                return dst;
            } );

            //https://tools.ietf.org/html/rfc2911#section-4.4
            mapper.CreateMap<IDictionary<string, IppAttribute[]>, GetPrinterAttributesResponse>((src, map) =>
                new GetPrinterAttributesResponse
                {
                    CharsetConfigured = map.MapFromDic<string?>(src, PrinterAttribute.CharsetConfigured),
                    CharsetSupported = map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.CharsetSupported),
                    ColorSupported = map.MapFromDic<bool?>(src, PrinterAttribute.ColorSupported),
                    CompressionSupported =
                        map.MapFromDicSetNull<Compression[]?>(src, PrinterAttribute.CompressionSupported),
                    DocumentFormatDefault = map.MapFromDic<string?>(src, PrinterAttribute.DocumentFormatDefault),
                    DocumentFormatSupported =
                        map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.DocumentFormatSupported),
                    GeneratedNaturalLanguageSupported =
                        map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.GeneratedNaturalLanguageSupported),
                    IppVersionsSupported =
                        map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.IppVersionsSupported),
                    JobImpressionsSupported = map.MapFromDic<Range?>(src, PrinterAttribute.JobImpressionsSupported),
                    JobKOctetsSupported = map.MapFromDic<Range?>(src, PrinterAttribute.JobKOctetsSupported),
                    JobMediaSheetsSupported = map.MapFromDic<Range?>(src, PrinterAttribute.JobMediaSheetsSupported),
                    MultipleDocumentJobsSupported =
                        map.MapFromDic<bool?>(src, PrinterAttribute.MultipleDocumentJobsSupported),
                    MultipleOperationTimeOut = map.MapFromDic<int?>(src, PrinterAttribute.MultipleOperationTimeOut),
                    NaturalLanguageConfigured =
                        map.MapFromDic<string?>(src, PrinterAttribute.NaturalLanguageConfigured),
                    OperationsSupported =
                        map.MapFromDicSetNull<IppOperation[]?>(src, PrinterAttribute.OperationsSupported),
                    PagesPerMinute = map.MapFromDic<int?>(src, PrinterAttribute.PagesPerMinute),
                    PdlOverrideSupported = map.MapFromDic<string?>(src, PrinterAttribute.PdlOverrideSupported),
                    PagesPerMinuteColor = map.MapFromDic<int?>(src, PrinterAttribute.PagesPerMinuteColor),
                    PrinterCurrentTime = map.MapFromDic<DateTimeOffset?>(src, PrinterAttribute.PrinterCurrentTime),
                    PrinterDriverInstaller = map.MapFromDic<string?>(src, PrinterAttribute.PrinterDriverInstaller),
                    PrinterInfo = map.MapFromDic<string?>(src, PrinterAttribute.PrinterInfo),
                    PrinterIsAcceptingJobs = map.MapFromDic<bool?>(src, PrinterAttribute.PrinterIsAcceptingJobs),
                    PrinterLocation = map.MapFromDic<string?>(src, PrinterAttribute.PrinterLocation),
                    PrinterMakeAndModel = map.MapFromDic<string?>(src, PrinterAttribute.PrinterMakeAndModel),
                    PrinterMessageFromOperator =
                        map.MapFromDic<string?>(src, PrinterAttribute.PrinterMessageFromOperator),
                    PrinterMoreInfo = map.MapFromDic<string?>(src, PrinterAttribute.PrinterMoreInfo),
                    PrinterMoreInfoManufacturer =
                        map.MapFromDic<string?>(src, PrinterAttribute.PrinterMoreInfoManufacturer),
                    PrinterName = map.MapFromDic<string?>(src, PrinterAttribute.PrinterName),
                    PrinterState = map.MapFromDic<PrinterState?>(src, PrinterAttribute.PrinterState),
                    PrinterStateMessage = map.MapFromDic<string?>(src, PrinterAttribute.PrinterStateMessage),
                    PrinterStateReasons =
                        map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.PrinterStateReasons),
                    PrinterUpTime = map.MapFromDic<int?>(src, PrinterAttribute.PrinterUpTime),
                    PrinterUriSupported =
                        map.MapFromDicSetNull<string[]?>(src, PrinterAttribute.PrinterUriSupported),
                    PrintScalingDefault = map.MapFromDic<PrintScaling?>(src, PrinterAttribute.PrintScalingDefault),
                    PrintScalingSupported =
                        map.MapFromDicSetNull<PrintScaling[]?>(src, PrinterAttribute.PrintScalingSupported),
                    QueuedJobCount = map.MapFromDic<int?>(src, PrinterAttribute.QueuedJobCount),
                    ReferenceUriSchemesSupported =
                        map.MapFromDicSetNull<UriScheme[]?>(src, PrinterAttribute.ReferenceUriSchemesSupported),
                    UriAuthenticationSupported =
                        map.MapFromDicSetNull<UriAuthentication[]?>(src, PrinterAttribute.UriAuthenticationSupported),
                    UriSecuritySupported =
                        map.MapFromDicSetNull<UriSecurity[]?>(src, PrinterAttribute.UriSecuritySupported),
                    MediaDefault = map.MapFromDic<string?>(src, PrinterAttribute.MediaDefault),
                    MediaColDefault = map.MapFromDic<string?>( src, PrinterAttribute.MediaColDefault ),
                    MediaSupported = map.MapFromDicSetNull<string[]?>( src, PrinterAttribute.MediaSupported ),
                    SidesDefault = map.MapFromDic<Sides?>( src, PrinterAttribute.SidesDefault ),
                    SidesSupported = map.MapFromDicSetNull<Sides[]?>( src, PrinterAttribute.SidesSupported ),
                    FinishingsDefault = map.MapFromDic<Finishings?>( src, PrinterAttribute.FinishingsDefault ),
                    PrinterResolutionDefault = map.MapFromDic<Resolution?>( src, PrinterAttribute.PrinterResolutionDefault ),
                    PrinterResolutionSupported = map.MapFromDicSetNull<Resolution[]?>( src, PrinterAttribute.PrinterResolutionSupported ),
                    PrintQualityDefault = map.MapFromDic<PrintQuality?>( src, PrinterAttribute.PrintQualityDefault ),
                    PrintQualitySupported = map.MapFromDicSetNull<PrintQuality[]?>( src, PrinterAttribute.PrintQualitySupported ),
                    JobPriorityDefault = map.MapFromDic<int?>( src, PrinterAttribute.JobPriorityDefault ),
                    JobPrioritySupported = map.MapFromDic<int?>( src, PrinterAttribute.JobPrioritySupported ),
                    CopiesDefault = map.MapFromDic<int?>( src, PrinterAttribute.CopiesDefault ),
                    CopiesSupported = map.MapFromDic<Range?>( src, PrinterAttribute.CopiesSupported ),
                    OrientationRequestedDefault = map.MapFromDic<Orientation?>( src, PrinterAttribute.OrientationRequestedDefault ),
                    OrientationRequestedSupported = map.MapFromDicSetNull<Orientation[]?>( src, PrinterAttribute.OrientationRequestedSupported ),
                    PageRangesSupported = map.MapFromDic<bool?>( src, PrinterAttribute.PageRangesSupported ),
                    JobHoldUntilDefault = map.MapFromDic<JobHoldUntil?>( src, PrinterAttribute.JobHoldUntilDefault ),
                    JobHoldUntilSupported = map.MapFromDicSetNull<JobHoldUntil[]?>( src, PrinterAttribute.JobHoldUntilSupported )
                } );

            mapper.CreateMap<GetPrinterAttributesResponse, IDictionary<string, IppAttribute[]>>( ( src, map ) =>
                {
                    var dic = new Dictionary<string, IppAttribute[]>();
                    if(src.CharsetConfigured != null)
                        dic.Add(PrinterAttribute.CharsetConfigured, new IppAttribute[] { new IppAttribute( Tag.Charset, PrinterAttribute.CharsetConfigured, src.CharsetConfigured ) });
                    if ( src.CharsetSupported?.Any() ?? false )
                        dic.Add(PrinterAttribute.CharsetSupported, src.CharsetSupported.Select( x => new IppAttribute( Tag.Charset, PrinterAttribute.CharsetSupported, x ) ).ToArray());
                    if( src.ColorSupported != null )
                        dic.Add( PrinterAttribute.ColorSupported, new IppAttribute[] { new IppAttribute( Tag.Boolean, PrinterAttribute.ColorSupported, src.ColorSupported.Value ) } );
                    if ( src.CompressionSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.CompressionSupported, src.CompressionSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.CompressionSupported, map.Map<string>( x ) ) ).ToArray() );
                    if ( src.DocumentFormatDefault != null )
                        dic.Add( PrinterAttribute.DocumentFormatDefault, new IppAttribute[] { new IppAttribute( Tag.MimeMediaType, PrinterAttribute.DocumentFormatDefault, src.DocumentFormatDefault ) } );
                    if ( src.DocumentFormatSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.DocumentFormatSupported, src.DocumentFormatSupported.Select( x => new IppAttribute( Tag.MimeMediaType, PrinterAttribute.DocumentFormatSupported, x ) ).ToArray() );
                    if ( src.GeneratedNaturalLanguageSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.GeneratedNaturalLanguageSupported, src.GeneratedNaturalLanguageSupported.Select( x => new IppAttribute( Tag.NaturalLanguage, PrinterAttribute.GeneratedNaturalLanguageSupported, x ) ).ToArray() );
                    if ( src.IppVersionsSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.IppVersionsSupported, src.IppVersionsSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.IppVersionsSupported, x ) ).ToArray() );
                    if ( src.JobImpressionsSupported != null )
                        dic.Add( PrinterAttribute.JobImpressionsSupported, new IppAttribute[] { new IppAttribute( Tag.RangeOfInteger, PrinterAttribute.JobImpressionsSupported, src.JobImpressionsSupported.Value ) } );
                    if ( src.JobKOctetsSupported != null )
                        dic.Add( PrinterAttribute.JobKOctetsSupported, new IppAttribute[] { new IppAttribute( Tag.RangeOfInteger, PrinterAttribute.JobKOctetsSupported, src.JobKOctetsSupported.Value ) } );
                    if ( src.JobMediaSheetsSupported != null )
                        dic.Add( PrinterAttribute.JobMediaSheetsSupported, new IppAttribute[] { new IppAttribute( Tag.RangeOfInteger, PrinterAttribute.JobMediaSheetsSupported, src.JobMediaSheetsSupported.Value ) } );
                    if ( src.MultipleDocumentJobsSupported != null )
                        dic.Add( PrinterAttribute.MultipleDocumentJobsSupported, new IppAttribute[] { new IppAttribute( Tag.Boolean, PrinterAttribute.MultipleDocumentJobsSupported, src.MultipleDocumentJobsSupported.Value ) } );
                    if ( src.MultipleOperationTimeOut != null )
                        dic.Add( PrinterAttribute.MultipleOperationTimeOut, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.MultipleOperationTimeOut, src.MultipleOperationTimeOut.Value ) } );
                    if ( src.NaturalLanguageConfigured != null )
                        dic.Add( PrinterAttribute.NaturalLanguageConfigured, new IppAttribute[] { new IppAttribute( Tag.NaturalLanguage, PrinterAttribute.NaturalLanguageConfigured, src.NaturalLanguageConfigured ) } );
                    if ( src.OperationsSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.OperationsSupported, src.OperationsSupported.Select( x => new IppAttribute( Tag.Enum, PrinterAttribute.OperationsSupported, (int)x ) ).ToArray() );
                    if ( src.PagesPerMinute != null )
                        dic.Add( PrinterAttribute.PagesPerMinute, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.PagesPerMinute, src.PagesPerMinute.Value ) } );
                    if ( src.PdlOverrideSupported != null )
                        dic.Add( PrinterAttribute.PdlOverrideSupported, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.PdlOverrideSupported, src.PdlOverrideSupported ) } );
                    if ( src.PagesPerMinuteColor != null )
                        dic.Add( PrinterAttribute.PagesPerMinuteColor, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.PagesPerMinuteColor, src.PagesPerMinuteColor.Value ) } );
                    if ( src.PrinterCurrentTime != null )
                        dic.Add( PrinterAttribute.PrinterCurrentTime, new IppAttribute[] { new IppAttribute( Tag.DateTime, PrinterAttribute.PrinterCurrentTime, src.PrinterCurrentTime.Value ) } );
                    if ( src.PrinterDriverInstaller != null )
                        dic.Add( PrinterAttribute.PrinterDriverInstaller, new IppAttribute[] { new IppAttribute( Tag.Uri, PrinterAttribute.PrinterDriverInstaller, src.PrinterDriverInstaller ) } );
                    if ( src.PrinterInfo != null )
                        dic.Add( PrinterAttribute.PrinterInfo, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, PrinterAttribute.PrinterInfo, src.PrinterInfo ) } );
                    if ( src.PrinterIsAcceptingJobs != null )
                        dic.Add( PrinterAttribute.PrinterIsAcceptingJobs, new IppAttribute[] { new IppAttribute( Tag.Boolean, PrinterAttribute.PrinterIsAcceptingJobs, src.PrinterIsAcceptingJobs.Value ) } );
                    if ( src.PrinterLocation != null )
                        dic.Add( PrinterAttribute.PrinterLocation, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, PrinterAttribute.PrinterLocation, src.PrinterLocation ) } );
                    if ( src.PrinterMakeAndModel != null )
                        dic.Add( PrinterAttribute.PrinterMakeAndModel, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, PrinterAttribute.PrinterMakeAndModel, src.PrinterMakeAndModel ) } );
                    if ( src.PrinterMessageFromOperator != null )
                        dic.Add( PrinterAttribute.PrinterMessageFromOperator, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, PrinterAttribute.PrinterMessageFromOperator, src.PrinterMessageFromOperator ) } );
                    if ( src.PrinterMoreInfo != null )
                        dic.Add( PrinterAttribute.PrinterMoreInfo, new IppAttribute[] { new IppAttribute( Tag.Uri, PrinterAttribute.PrinterMoreInfo, src.PrinterMoreInfo ) } );
                    if ( src.PrinterMoreInfoManufacturer != null )
                        dic.Add( PrinterAttribute.PrinterMoreInfoManufacturer, new IppAttribute[] { new IppAttribute( Tag.Uri, PrinterAttribute.PrinterMoreInfoManufacturer, src.PrinterMoreInfoManufacturer ) } );
                    if ( src.PrinterName != null )
                        dic.Add( PrinterAttribute.PrinterName, new IppAttribute[] { new IppAttribute( Tag.NameWithoutLanguage, PrinterAttribute.PrinterName, src.PrinterName ) } );
                    if ( src.PrinterState != null )
                        dic.Add( PrinterAttribute.PrinterState, new IppAttribute[] { new IppAttribute( Tag.Enum, PrinterAttribute.PrinterState, (int)src.PrinterState.Value ) } );
                    if ( src.PrinterStateMessage != null )
                        dic.Add( PrinterAttribute.PrinterStateMessage, new IppAttribute[] { new IppAttribute( Tag.TextWithoutLanguage, PrinterAttribute.PrinterStateMessage, src.PrinterStateMessage ) } );
                    if ( src.PrinterStateReasons?.Any() ?? false )
                        dic.Add( PrinterAttribute.PrinterStateReasons, src.PrinterStateReasons.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.PrinterStateReasons, x ) ).ToArray() );
                    if ( src.PrinterUpTime != null )
                        dic.Add( PrinterAttribute.PrinterUpTime, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.PrinterUpTime, src.PrinterUpTime.Value ) } );
                    if ( src.PrinterUriSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.PrinterUriSupported, src.PrinterUriSupported.Select( x => new IppAttribute( Tag.Uri, PrinterAttribute.PrinterUriSupported, x ) ).ToArray() );
                    if ( src.PrintScalingDefault != null )
                        dic.Add( PrinterAttribute.PrintScalingDefault, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.PrintScalingDefault, map.Map<string>( src.PrintScalingDefault ) ) } );
                    if ( src.PrintScalingSupported != null )
                        dic.Add( PrinterAttribute.PrintScalingSupported, src.PrintScalingSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.PrintScalingSupported, map.Map<string>( x ) ) ).ToArray() );
                    if ( src.QueuedJobCount != null )
                        dic.Add( PrinterAttribute.QueuedJobCount, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.QueuedJobCount, src.QueuedJobCount.Value ) } );
                    if ( src.ReferenceUriSchemesSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.ReferenceUriSchemesSupported, src.ReferenceUriSchemesSupported.Select( x => new IppAttribute( Tag.UriScheme, PrinterAttribute.ReferenceUriSchemesSupported, map.Map<string>( x ) ) ).ToArray() );
                    if ( src.UriAuthenticationSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.UriAuthenticationSupported, src.UriAuthenticationSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.UriAuthenticationSupported, map.Map<string>( x ) ) ).ToArray() );
                    if ( src.UriSecuritySupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.UriSecuritySupported, src.UriSecuritySupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.UriSecuritySupported, map.Map<string>( x ) ) ).ToArray() );
                    if( src.MediaDefault != null )
                        dic.Add( PrinterAttribute.MediaDefault, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.MediaDefault, src.MediaDefault ) } );
                    if ( src.MediaColDefault != null )
                        dic.Add( PrinterAttribute.MediaColDefault, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.MediaColDefault, src.MediaColDefault ) } );
                    if ( src.MediaSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.MediaSupported, src.MediaSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.MediaSupported, x ) ).ToArray() );
                    if ( src.SidesDefault != null )
                        dic.Add( PrinterAttribute.SidesDefault, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.SidesDefault, map.Map<string>( src.SidesDefault ) ) } );
                    if ( src.SidesSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.SidesSupported, src.SidesSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.SidesSupported, map.Map<string>( x ) ) ).ToArray() );
                    if ( src.FinishingsDefault != null )
                        dic.Add( PrinterAttribute.FinishingsDefault, new IppAttribute[] { new IppAttribute( Tag.Enum, PrinterAttribute.FinishingsDefault, (int)src.FinishingsDefault.Value ) } );
                    if ( src.PrinterResolutionDefault != null )
                        dic.Add( PrinterAttribute.PrinterResolutionDefault, new IppAttribute[] { new IppAttribute( Tag.Resolution, PrinterAttribute.PrinterResolutionDefault, src.PrinterResolutionDefault.Value ) } );
                    if ( src.PrinterResolutionSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.PrinterResolutionSupported, src.PrinterResolutionSupported.Select( x => new IppAttribute( Tag.Resolution, PrinterAttribute.PrinterResolutionSupported, x ) ).ToArray() );
                    if ( src.PrintQualityDefault != null )
                        dic.Add( PrinterAttribute.PrintQualityDefault, new IppAttribute[] { new IppAttribute( Tag.Enum, PrinterAttribute.PrintQualityDefault, (int)src.PrintQualityDefault.Value ) } );
                    if ( src.PrintQualitySupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.PrintQualitySupported, src.PrintQualitySupported.Select( x => new IppAttribute( Tag.Enum, PrinterAttribute.PrintQualitySupported, (int)x ) ).ToArray() );
                    if ( src.JobPriorityDefault != null )
                        dic.Add( PrinterAttribute.JobPriorityDefault, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.JobPriorityDefault, src.JobPriorityDefault.Value ) } );
                    if ( src.JobPrioritySupported != null )
                        dic.Add( PrinterAttribute.JobPrioritySupported, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.JobPrioritySupported, src.JobPrioritySupported.Value ) } );
                    if ( src.CopiesDefault != null )
                        dic.Add( PrinterAttribute.CopiesDefault, new IppAttribute[] { new IppAttribute( Tag.Integer, PrinterAttribute.CopiesDefault, src.CopiesDefault.Value ) } );
                    if ( src.CopiesSupported != null )
                        dic.Add( PrinterAttribute.CopiesSupported, new IppAttribute[] { new IppAttribute( Tag.RangeOfInteger, PrinterAttribute.CopiesSupported, src.CopiesSupported.Value ) } );
                    if ( src.OrientationRequestedDefault != null )
                        dic.Add( PrinterAttribute.OrientationRequestedDefault, new IppAttribute[] { new IppAttribute( Tag.Enum, PrinterAttribute.OrientationRequestedDefault, (int)src.OrientationRequestedDefault.Value ) } );
                    if ( src.OrientationRequestedSupported?.Any() ?? false )
                        dic.Add( PrinterAttribute.OrientationRequestedSupported, src.OrientationRequestedSupported.Select( x => new IppAttribute( Tag.Enum, PrinterAttribute.OrientationRequestedSupported, (int)x ) ).ToArray() );
                    if ( src.PageRangesSupported != null )
                        dic.Add( PrinterAttribute.PageRangesSupported, new IppAttribute[] { new IppAttribute( Tag.Boolean, PrinterAttribute.PageRangesSupported, src.PageRangesSupported.Value ) } );
                    if (src.JobHoldUntilDefault != null)
                        dic.Add( PrinterAttribute.JobHoldUntilDefault, new IppAttribute[] { new IppAttribute( Tag.Keyword, PrinterAttribute.JobHoldUntilDefault, map.Map<string>( src.JobHoldUntilDefault.Value ) ) } );
                    if (src.JobHoldUntilSupported?.Any() ?? false)
                        dic.Add( PrinterAttribute.JobHoldUntilSupported, src.JobHoldUntilSupported.Select( x => new IppAttribute( Tag.Keyword, PrinterAttribute.JobHoldUntilSupported, map.Map<string>( x ) ) ).ToArray() );
                    return dic;
                } );
        }
    }
}
