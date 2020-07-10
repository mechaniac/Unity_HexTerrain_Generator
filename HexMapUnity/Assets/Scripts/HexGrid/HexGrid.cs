using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int tilesPerChunkSide = 4;

    public int chunkCountX = 3;
    public int chunkCountZ = 3;

    int tilesX;
    int tilesZ;


    [SerializeField]
    HexTile hexTilePrefab;

    [HideInInspector]
    public HexTile[] tiles;

    [SerializeField]
    Text tileLabelPrefab;

    Canvas gridCanvas;
    [SerializeField]
    HexMeshChunk chunkPrefab;

    [HideInInspector]
    HexMeshChunk[] chunks;

    private void Awake()
    {
        tilesX = chunkCountX * tilesPerChunkSide;
        tilesZ = chunkCountZ * tilesPerChunkSide;

        gridCanvas = GetComponentInChildren<Canvas>();

        tiles = new HexTile[tilesX * tilesZ];
        GameObject tilesHolder = new GameObject("tilesHolder");
        tilesHolder.transform.parent = transform;
        for (int z = 0, i = 0; z < tilesZ; z++)
        {
            for (int x = 0; x < tilesX; x++, i++)
            {
                CreateTile(i, x, z, tilesHolder.transform);
            }
        }

        chunks = new HexMeshChunk[chunkCountX * chunkCountZ];
        GameObject chunkHolder = new GameObject("chunkHolder");
        chunkHolder.transform.parent = transform;
        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++, i++)
            {
                CreateChunk(i, x, z, chunkHolder.transform);
            }
        }
    }

    private void Start()
    {
        SetTileNeighbours();
        //HexTile t = GetTileFromCubicCoordinates(21, 17);
        //Debug.Log($"tile {t.coordinates}");
        //Debug.Log($"tile {t.index}");
    }

    void CreateTile(int i, int x, int z, Transform parent)
    {
        HexTile t = tiles[i] = Instantiate(hexTilePrefab);
        t.transform.parent = parent;
        t.name = $"tile {i} {x}/{z}";
        Vector3 position;
        position.x = (x + z * .5f - (z / 2)) * HexMetrics.innerRadius * 2f;
        position.y = 0;
        position.z = z * HexMetrics.outerRadius * 1.5f;
        t.transform.position = position;
        t.basicX = x;
        t.basicZ = z;
        t.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        t.index = i;
        SetLabel($"{x}\n{z}", Color.black, 0, false, 2);
        SetLabel(t.coordinates.X.ToString(), Color.red, 240, true, 4);
        SetLabel(t.coordinates.Z.ToString(), Color.blue, 0, true, 4);
        SetLabel(t.coordinates.Y.ToString(), Color.green, 120, true, 4);

        void SetLabel(string input, Color textColor, int degree, bool isOffset, int size)
        {
            Vector2 offset = new Vector2();
            if (isOffset)
            {
                offset = Quaternion.Euler(0, 0, degree) * new Vector2(-HexMetrics.innerCornerRadius / 2f, 0);
            }

            Text l = Instantiate<Text>(tileLabelPrefab);
            l.rectTransform.SetParent(gridCanvas.transform, false);
            l.color = textColor;
            l.fontSize = size;
            l.rectTransform.anchoredPosition = new Vector2(t.transform.position.x, t.transform.position.z) + offset;
            l.text = input;
        }
    }

    void SetTileNeighbours()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            AssignNeighbours(tiles[i]);
        }


        void AssignNeighbours(HexTile t)
        {
            t.neighbours = new HexTile[6];

            int endOfX = chunkCountX * tilesPerChunkSide - 1;

            if (t.basicX != endOfX)
            {
                t.neighbours[0] = GetTileFromCubicCoordinates(t.coordinates.X + 1, t.coordinates.Z);
            }

            if (!(t.basicX == endOfX && t.basicZ % 2 == 1))
            {
                t.neighbours[1] = GetTileFromCubicCoordinates(t.coordinates.X, t.coordinates.Z + 1);
            }

            if (!(t.basicX == 0 && t.basicZ % 2 == 0))
            {
                t.neighbours[2] = GetTileFromCubicCoordinates(t.coordinates.X - 1, t.coordinates.Z + 1);
            }

            if (t.basicX != 0)
            {
                t.neighbours[3] = GetTileFromCubicCoordinates(t.coordinates.X - 1, t.coordinates.Z);
            }

            if (!(t.basicX == 0 && t.basicZ % 2 == 0))
            {
                t.neighbours[4] = GetTileFromCubicCoordinates(t.coordinates.X, t.coordinates.Z - 1);
            }

            if (!(t.basicX == endOfX && t.basicZ % 2 == 1))
            {
                t.neighbours[5] = GetTileFromCubicCoordinates(t.coordinates.X + 1, t.coordinates.Z - 1);
            }


        }
    }

    HexTile GetTileFromCubicCoordinates(int x, int z, int y = 0)
    {
        int i = x + z * tilesX + z / 2;

        if (i >= tiles.Length || i < 0) return null;

        if (tiles[i] != null)
        {
            return tiles[i];
        }
        return null;
    }

    void CreateChunk(int i, int x, int z, Transform parent)
    {
        HexMeshChunk c = chunks[i] = Instantiate(chunkPrefab);
        c.transform.parent = parent;
        c.name = $"chunk {i} {x}/{z}";
        Vector3 position;
        position.x = x * HexMetrics.outerRadius * chunkCountX;
        position.y = 0;
        position.z = z * HexMetrics.innerRadius * chunkCountZ;
        c.index = i;
        c.grid = this;
    }
}


