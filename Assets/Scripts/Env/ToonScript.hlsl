#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

coid LightingCelShaded_float(out float3 Color) {
#if defined(SHADERGRAPH_PREVIEW)
    ...
#else
    Color =
    float3(0.5f, 0.5f, 0.5f);
#endif
}

#endif

#ifndef UNIVERSAL_REALTIME_LIGHTS_INCLUDED
#define UNIRVERSAL_REALTIME_LIGHTS_INCLUDED

...

uint GetMeshRenderingLightLayer() { ... }

struct Light { ... };

...

float DistanceAttenuation( ... ) { ... }
half AngleAttenuation(... ) { ... }

Light GetMainlight() { ... }
Light GetMainLight( ... ) { ... }
Light GetMainLight( ... ) { ... }
Light GetMainLight( ... ) { ... }

Light GetAdditionalPerObjectLight( ... ) { ... }
uint GetPerObjectLightIndexOffset() { ... }
int GetPerObjectLightIndex(... ) { ... }

Light GetAdditionalLight( ... ) { ... }
Light GetAdditionalLight( ... ) { ... }
Light GetAdditionalLight( ... ) { ... }

int GetAdditionalLightsCount() { ... }

half4 CalculateShadowMask(... ) { ... }