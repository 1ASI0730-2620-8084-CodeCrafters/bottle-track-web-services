using BottleTrack.API.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BottleTrack.API.Shared.Infrastructure.Interfaces.Asp.Configuration;

public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    private static AttributeRouteModel? ReplaceControllerTemplate(SelectorModel selector, string name)
    {
        if (selector.AttributeRouteModel == null)
            return null;

        AttributeRouteModel route = new AttributeRouteModel();
        if (selector.AttributeRouteModel.Template != null)
            route.Template = selector.AttributeRouteModel.Template.Replace("[controller]", name.ToKebabCase());

        return route;
    }

    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);

        foreach (var action in controller.Actions)
        {
            foreach (var selector in action.Selectors)
                selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);
        }
    }
}
