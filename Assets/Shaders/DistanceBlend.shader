// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DistanceBlend"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Color0("Color0", Color) = (0,0,0,0)
		_Color1("Color1", Color) = (0,0,0,1)
		_PlayerPos("PlayerPos", Vector) = (0.5,0.5,0,0)
		_Multiplier("Multiplier", Float) = 5

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord2 : TEXCOORD2;
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float _Multiplier;
			uniform float2 _PlayerPos;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord2 = screenPos;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float4 lerpResult4 = lerp( _Color0 , _Color1 , ( _Multiplier * sqrt( ( pow( ( _PlayerPos.x - ase_screenPosNorm.x ) , (float)2 ) + pow( ( _PlayerPos.y - ase_screenPosNorm.y ) , (float)2 ) ) ) ));
				
				half4 color = lerpResult4;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17800
42;458;982;564;1260.329;361.9007;1.66268;True;False
Node;AmplifyShaderEditor.Vector2Node;39;-748.4019,158.3571;Inherit;False;Property;_PlayerPos;PlayerPos;2;0;Create;True;0;0;False;0;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ScreenPosInputsNode;14;-740.9388,325.013;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;43;-537.834,218.1796;Inherit;False;Constant;_Int0;Int 0;3;0;Create;True;0;0;False;0;2;0;0;1;INT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;-532.909,118.7953;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;46;-525.4973,330.4599;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-373.343,164.3687;Inherit;False;False;2;0;FLOAT;0;False;1;INT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;44;-358.9315,361.0334;Inherit;False;False;2;0;FLOAT;0;False;1;INT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-196.6874,293.562;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SqrtOpNode;48;-285.7526,48.14249;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-294.3627,-29.00266;Inherit;False;Property;_Multiplier;Multiplier;3;0;Create;True;0;0;False;0;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-174.9174,-465.7653;Inherit;False;Property;_Color1;Color1;1;0;Create;True;0;0;False;0;0,0,0,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-96.96925,-120.3304;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;100.6829,-549.994;Inherit;False;Property;_Color0;Color0;0;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;25;-786.2548,-5.45694;Inherit;False;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;4;105.201,-266.005;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;6;410.6791,-268.4344;Float;False;True;-1;2;ASEMaterialInspector;0;6;DistanceBlend;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;True;True;True;True;True;0;True;-9;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;41;0;39;1
WireConnection;41;1;14;1
WireConnection;46;0;39;2
WireConnection;46;1;14;2
WireConnection;42;0;41;0
WireConnection;42;1;43;0
WireConnection;44;0;46;0
WireConnection;44;1;43;0
WireConnection;47;0;42;0
WireConnection;47;1;44;0
WireConnection;48;0;47;0
WireConnection;49;0;51;0
WireConnection;49;1;48;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;4;2;49;0
WireConnection;6;0;4;0
ASEEND*/
//CHKSM=809A534BCFDE693F9FDD0CEFF5D7B93ACBBE67EC