using UnityEngine;
using System.Collections;

public class CreateBoosterTileOnDrag : MonoBehaviour {

    //public
    public Tile.TileType tileType;
    public GameObject ghost;

    //private
    bool dragging = false;
    Map map;
    GameObject currentGhost;
    SpriteRenderer ghostRenderer;
    PlayerBoosterTiles playerBoosterTiles;

	void Start () {
        map = Map.instance;
        playerBoosterTiles = PlayerBoosterTiles.instance;
        gameObject.AddComponent<BoxCollider2D>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(140f, 140f);
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
                dragging = true;
            }
        } else if (Input.GetMouseButton(0) && dragging) {
            if (GetClosestCell() != null) {
                DrawGhostAtClosestInputPos();
            } else if (currentGhost != null) {
                DestroyGhost();
            }

        } else if (Input.GetMouseButtonUp(0) && dragging) {
            PlaceTile();
            DestroyGhost();
            dragging = false;
        }
	}

    void PlaceTile() {
        Cell closetCell = GetClosestCell();
        if (closetCell == null) {
            //User input is too far from the map to place a tower
            return;
        }
        if(closetCell.tile.tileType != Tile.TileType.NORMAL){
            DisplayCannotPlaceTileMessage();
            return;
        }
        closetCell.tile.tileType = tileType;
        if (closetCell.localizableOnMap != null) {
            PlasmaBoostable plasmaBoostable = closetCell.localizableOnMap.GetComponent<PlasmaBoostable>();
            plasmaBoostable.AddDamageMultiplierIfOnDamageBooster();
        }
        if (tileType == Tile.TileType.DAMAGE_BOOSTER) {
            playerBoosterTiles.CurrentDamageBoosterAmount --;
        } else if (tileType == Tile.TileType.RANGE_BOOSTER) {
            playerBoosterTiles.CurrentRangeBoosterAmount --;
        }

        //TODO
        //Update current tower if present (might need work)
    }

    void DisplayCannotPlaceTileMessage(){
        //TODO
    }

    void DestroyGhost() {
        Destroy(currentGhost);
    }

    void DrawGhostAtClosestInputPos() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (map.GetCellAt((int)(mousePos.x / map.cellSize), (int)(mousePos.y / map.cellSize)) != null) {
            Vector3 closestPos = map.GetCellPos(GetClosestCell());
            if (currentGhost == null) {
                currentGhost = Instantiate(ghost, closestPos, Quaternion.identity) as GameObject;
                ghostRenderer = currentGhost.GetComponent<SpriteRenderer>();
            }
            currentGhost.transform.position = closestPos;
            ghostRenderer.color = Tile.GetTileTypeColor(tileType);
        }
    }

    Cell GetClosestCell() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return map.GetCellClosestToPos(mousePos.x, mousePos.y + map.cellSize);
    }
}
