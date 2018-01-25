// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MiKoShader" {
// miko@mikoweb.de 12/2012

Properties
{
_MainTex ("TileTexture", 2D) = "white" {}
_HeightTex ("HeightMap", 2D) = "white" {}
}

SubShader
{
LOD 200

Pass
{
CGPROGRAM

#pragma only_renderers d3d11
#pragma target 4.0

#include "UnityCG.cginc"

#pragma vertex myVertexShader
#pragma geometry myGeometryShader
#pragma fragment myFragmentShader

#define MAP_SIZE 128.0f
#define HEX_SIDE 1.0f
#define SQRT3 1.73205081f
#define DIST 0.00f

#define HEIGHT_WATER 0.00f
#define HEIGHT_FIELD 0.25f
#define HEIGHT_HILL 0.50f
#define HEIGHT_MOUNTAIN 0.75f

#define HM_WATER 0.0f
#define HM_FIELD 64.0f
#define HM_HILL 128.0f
#define HM_MOUNTAIN 192.0f

struct vIn // Into the vertex shader
{
float4 vertex : POSITION;
float4 color : COLOR0;
};

struct gIn // OUT vertex shader, IN geometry shader
{
float4 pos : SV_POSITION;
float4 col : COLOR0;
};

struct v2f // OUT geometry shader, IN fragment shader
{
float4 pos : SV_POSITION;
float2 uv_MainTex : TEXCOORD0;
};

float4 _MainTex_ST;
sampler2D _MainTex;
sampler2D _HeightTex;

// ----------------------------------------------------
gIn myVertexShader(vIn v)
{
gIn o; // Out here, into geometry shader
UNITY_INITIALIZE_OUTPUT(gIn,o);

// Passing on color to next shader (using .r/.g there as tile coordinate)
o.col = v.color;

// Passing on center vertex (tile to be built by geometry shader from it later)
o.pos = v.vertex;

return o;
}

// ----------------------------------------------------
// Local routine to retrieve Y-height from terrain type (= heightmap-coded value)
float _getTerrainHeight(float fType)
{
if (fType < HM_WATER+1) return(HEIGHT_WATER);
else if (fType < HM_FIELD+1) return(HEIGHT_FIELD);
else if (fType < HM_HILL+1) return(HEIGHT_HILL);
else return(HEIGHT_MOUNTAIN);
}

[maxvertexcount(20)] // 8 for hexagon tile triangles, 12 for walls (at max)
// ----------------------------------------------------
// Using "point" type as input, not "triangle"
void myGeometryShader(point gIn vert[1], inout TriangleStream<v2f> triStream)
{
// The 6 vertex positions of a hex tile, in respect to the center point
const float4 vc[6] = { float4(-0.5f*SQRT3*HEX_SIDE+DIST, 0.0f, 0.5f*HEX_SIDE-DIST, 0.0f),
float4( 0.0f, 0.0f, HEX_SIDE-DIST, 0.0f),
float4( 0.5f*SQRT3*HEX_SIDE-DIST, 0.0f, 0.5f*HEX_SIDE-DIST, 0.0f),
float4( 0.5f*SQRT3*HEX_SIDE-DIST, 0.0f, -0.5f*HEX_SIDE+DIST, 0.0f),
float4( 0.0f, 0.0f, -HEX_SIDE+DIST, 0.0f),
float4(-0.5f*SQRT3*HEX_SIDE+DIST, 0.0f, -0.5f*HEX_SIDE+DIST, 0.0f) };

// The UV values for the different types of terrains. Using a 512x128 pixel texture
const float2 UV1[6] = { float2( 0.0f/512.0f, 95.0f/128.0f ),
float2( 55.0f/512.0f, 127.0f/128.0f ),
float2( 110.0f/512.0f, 95.0f/128.0f ),
float2( 110.0f/512.0f, 31.0f/128.0f ),
float2( 55.0f/512.0f, 0.0f/128.0f ),
float2( 0.0f/512.0f, 31.0f/128.0f ) };

const float2 UV2[6] = { float2( (0.0f+120.0f)/512.0f, 95.0f/128.0f ),
float2( (55.0f+120.0f)/512.0f, 127.0f/128.0f ),
float2( (110.0f+120.0f)/512.0f, 95.0f/128.0f ),
float2( (110.0f+120.0f)/512.0f, 31.0f/128.0f ),
float2( (55.0f+120.0f)/512.0f, 0.0f/128.0f ),
float2( (0.0f+120.0f)/512.0f, 31.0f/128.0f ) };

const float2 UV3[6] = { float2( (0.0f+240.0f)/512.0f, 95.0f/128.0f ),
float2( (55.0f+240.0f)/512.0f, 127.0f/128.0f ),
float2( (110.0f+240.0f)/512.0f, 95.0f/128.0f ),
float2( (110.0f+240.0f)/512.0f, 31.0f/128.0f ),
float2( (55.0f+240.0f)/512.0f, 0.0f/128.0f ),
float2( (0.0f+240.0f)/512.0f, 31.0f/128.0f ) };

const float2 UV4[6] = { float2( (0.0f+360.0f)/512.0f, 95.0f/128.0f ),
float2( (55.0f+360.0f)/512.0f, 127.0f/128.0f ),
float2( (110.0f+360.0f)/512.0f, 95.0f/128.0f ),
float2( (110.0f+360.0f)/512.0f, 31.0f/128.0f ),
float2( (55.0f+360.0f)/512.0f, 0.0f/128.0f ),
float2( (0.0f+360.0f)/512.0f, 31.0f/128.0f ) };

const float2 UV_WALL = float2( (500.0f)/512.0f, 63.0f/128.0f );

const int TRI_STRIP_HEX[8] = { 1,2,0,3,4, 4,5,0 };
const int TRI_STRIP_WALL[4] = { 0,1,3,2 };

// Hextile directions to check for possible walls: left-downleft-downright
const float2 WALL_DIR[3] = { float2(-1.0f, 0.0f),
float2(-1.0f, -1.0f),
float2( 1.0f, -1.0f) };

const int VERT_POS_WALL[3][4] = { {0,5,5,0}, {5,4,4,5}, {4,3,3,4} };


// Get height map coordinates of this vertex, encoded via
// RGB: .r is Map.X, .g is Map.Z
float2 vTileCoords = float2(vert[0].col.r, vert[0].col.g);

// Real tile integer indices in respect to map size
float2 vTileCoordsXL = vTileCoords * (MAP_SIZE-1.0f);
vTileCoordsXL.x = round(vTileCoordsXL.x);
vTileCoordsXL.y = round(vTileCoordsXL.y);

// Retrieving the height from the heightmap
float4 tc = tex2Dlod(_HeightTex, float4(vTileCoords,0,0));
float fHeight = tc.x * 255.0f; // To be compared to the HM_xxx constants later

v2f v[6];

// Assign new vertices positions (6 new tile vertices, forming hexagon)
for (int i=0;i<6;i++) { v[i].pos = vert[0].pos + vc[i]; }

// Assign vertex height (y)
for (int i=0;i<6;i++) { v[i].pos.y = _getTerrainHeight(fHeight); }

// Assign UV values, respective to terrain type (to show different textures)
if (fHeight < HM_WATER+1) for (int i=0;i<6;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV1[i],_MainTex);
else if (fHeight < HM_FIELD+1) for (int i=0;i<6;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV2[i],_MainTex);
else if (fHeight < HM_HILL+1) for (int i=0;i<6;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV3[i],_MainTex);
else for (int i=0;i<6;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV4[i],_MainTex);

// Position in view space
for (int i=0;i<6;i++) { v[i].pos = UnityObjectToClipPos(v[i].pos); }

// Build the hex tile by submitting triangle strip vertices
for (int i=0;i<5;i++) triStream.Append(v[TRI_STRIP_HEX[i]]);
triStream.RestartStrip();
for (int i=5;i<8;i++) triStream.Append(v[TRI_STRIP_HEX[i]]);
triStream.RestartStrip();

// Create the wall triangles for this hex tile. Only those sides that
// go *down* (read: when adjacent tile on this side is lower)
// For each tile, doing a check left-downleft-downright (in x-z grid)

for (int iWall=0;iWall<3;iWall++)
{
// Get coordinates of adjacent tile for wall-check
float2 vAdjTileCoords;
vAdjTileCoords.x = vTileCoordsXL.x + WALL_DIR[iWall].x;
vAdjTileCoords.y = vTileCoordsXL.y + WALL_DIR[iWall].y;
if ((iWall==1)&&(round(vTileCoordsXL.y)%2==1)) vAdjTileCoords.x += 1.0f;
if ((iWall==2)&&(round(vTileCoordsXL.y)%2==0)) vAdjTileCoords.x -= 1.0f;
vAdjTileCoords = vAdjTileCoords / (MAP_SIZE-1.0f);

if ((vAdjTileCoords.x >= 0.0f)&&(vAdjTileCoords.y >= 0.0f)) // In range, not at border
{
float4 tc1 = tex2Dlod(_HeightTex, float4(vAdjTileCoords,0,0));
float fAdjHeight = tc1.x * 255.0f; // To be compared to the HM_xxx constants later

if (abs(fAdjHeight-fHeight)>5) // Currently, height differences are, like, 64
{
v2f v[4];
v[0].pos = vert[0].pos + vc[VERT_POS_WALL[iWall][0]]; v[0].pos.y = _getTerrainHeight(fHeight);
v[1].pos = vert[0].pos + vc[VERT_POS_WALL[iWall][1]]; v[1].pos.y = _getTerrainHeight(fHeight);
v[2].pos = vert[0].pos + vc[VERT_POS_WALL[iWall][2]]; v[2].pos.y = _getTerrainHeight(fAdjHeight);
v[3].pos = vert[0].pos + vc[VERT_POS_WALL[iWall][3]]; v[3].pos.y = _getTerrainHeight(fAdjHeight);

// Position in view space
for (int i=0;i<4;i++) { v[i].pos = UnityObjectToClipPos(v[i].pos); }

// Set UV values
for (int i=0;i<4;i++) v[i].uv_MainTex = TRANSFORM_TEX(UV_WALL,_MainTex);

// Submit triangles of wall
for (int i=0;i<4;i++) triStream.Append(v[TRI_STRIP_WALL[i]]);
triStream.RestartStrip();
}
}
} // for...iWall
}

// ----------------------------------------------------
half4 myFragmentShader(v2f IN) : COLOR
{
// Not considering normals/light here. Just texture
half4 c = tex2D (_MainTex, IN.uv_MainTex);
return c;
}

ENDCG
}
}
FallBack "Diffuse"
}