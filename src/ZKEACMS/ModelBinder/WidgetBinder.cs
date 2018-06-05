using System;
using System.Threading.Tasks;
///* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using Easy.Extend;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ZKEACMS.Widget;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ZKEACMS.ModelBinder
{
    public class WidgetBinder : IModelBinder
    {
        private readonly ModelBinderProviderContext modelBinderProviderContext;
        private readonly ILoggerFactory _loggerFactory;
        public WidgetBinder(ModelBinderProviderContext context, ILoggerFactory loggerFactory)
        {
            modelBinderProviderContext = context;
            _loggerFactory = loggerFactory;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var assembly = bindingContext.ValueProvider.GetValue("AssemblyName");
            var viewModelType = bindingContext.ValueProvider.GetValue("ViewModelTypeName");
            var widgetViewModelType = WidgetBase.KnownWidgetModel[$"{assembly.FirstValue},{viewModelType.FirstValue}"];
            var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
            var modelMetadataProvider = bindingContext.HttpContext.RequestServices.GetService<IModelMetadataProvider>();
            bindingContext.ModelMetadata = modelMetadataProvider.GetMetadataForType(widgetViewModelType);
            foreach (var property in modelMetadataProvider.GetMetadataForProperties(widgetViewModelType))
            {
                propertyBinders.Add(property, modelBinderProviderContext.CreateBinder(property));
            }
            ComplexTypeModelBinder modelBinder = new ComplexTypeModelBinder(propertyBinders, _loggerFactory);
            return modelBinder.BindModelAsync(bindingContext);
        }
    }
}
