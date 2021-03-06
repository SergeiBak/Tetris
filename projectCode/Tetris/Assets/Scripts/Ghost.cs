using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate() // gets called after all other updates
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    // clear the ghost tile
    private void Clear()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePos = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePos, null);
        }
    }

    // copies cells from main piece - this ensures that ghost piece rotates along with main piece
    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    private void Drop()
    {
        Vector3Int pos = this.trackingPiece.position;

        int currentRow = pos.y;
        int bottom = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingPiece);

        for (int row = currentRow; row >= bottom; row--)
        {
            pos.y = row;

            if (this.board.isValidPosition(this.trackingPiece, pos))
            {
                this.position = pos;
            }
            else
            {
                break;
            }
        }

        this.board.Set(this.trackingPiece);
    }

    // set the ghost tile
    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePos = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePos, this.tile);
        }
    }
}
