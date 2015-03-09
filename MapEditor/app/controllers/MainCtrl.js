mapEditor.controller("MainCtrl", function($scope){

  //Cell types
  var EMPTY_CELL = "empty";
  var OBSTACLE_CELL = "obstacle";
  var DAMAGE_BOOST_CELL = "damage";
  var SPAWNER_CELL = "spawner";
  var GOAL_CELL = "goal";

  var textFile = null;

  init();

  function init(){
    exposeCellTypes();

    var mapModelFromLocalStorage = localStorage.getItem("map");
    if (mapModelFromLocalStorage !== null){
      $scope.mapModel = JSON.parse(mapModelFromLocalStorage);
      //Map gotten from localstorage, no init needed
    } else {
      initMapData(null);
    }

    $scope.onCellClick = onCellClick;
    $scope.copyMapToClipboard = copyMapToClipboard;
    setUpDragFromDesktop();
    setUpDownloadBtn();

    $scope.$watch('mapModel.width', function(){
      initMapData();
    });
    $scope.$watch('mapModel.height', function(){
      initMapData();
    });
  }

  function exposeCellTypes(){
    $scope.cellTypes = [
      {
        label: EMPTY_CELL,
        color: "rgb(230, 230, 230)"
      }, {
        label: OBSTACLE_CELL,
        color: "red"
      }, {
        label: DAMAGE_BOOST_CELL,
        color: "cyan"
      },
      {
        label: SPAWNER_CELL,
        color: "black"
      }, {
        label: GOAL_CELL,
        color: "white"
      }];
    $scope.selectCellType = selectCellType;
    $scope.selectedCellTypeLabel = OBSTACLE_CELL;
  }

  function selectCellType(cellType){
    $scope.selectedCellTypeLabel = cellType.label;
  }

  function initMapData(mapJsonFromFile){
    var fileName = $scope.mapModel ? $scope.mapModel.fileName : "";
    var width = $scope.mapModel ? $scope.mapModel.width : 15;
    var height = $scope.mapModel ? $scope.mapModel.height : 10;
    width = mapJsonFromFile ? mapJsonFromFile.dimensions.width : width;
    height = mapJsonFromFile ? mapJsonFromFile.dimensions.height : height;
    var cells = $scope.mapModel ? $scope.mapModel.cells : [];
    $scope.mapModel = {
      width: width,
      height: height,
      cells: cells,
      fileName: fileName
    };
    if (mapJsonFromFile){
      initCellMatrix(false);
    } else {
      initCellMatrix(true);
    }
    if (mapJsonFromFile){
      loadMapWithMapJsonFromFile(mapJsonFromFile);
    }
  }

  function loadMapWithMapJsonFromFile(mapJsonFromFile){
    _.each(mapJsonFromFile.cells.walls, function(cell){
      findCell(cell.x, cell.y).type = _.findWhere($scope.cellTypes, {label: OBSTACLE_CELL});
    });
    _.each(mapJsonFromFile.cells.damageBoosters, function(cell){
      findCell(cell.x, cell.y).type = _.findWhere($scope.cellTypes, {label: DAMAGE_BOOST_CELL});
    });
    _.each(mapJsonFromFile.cells.spawners, function(cell){
      findCell(cell.x, cell.y).type = _.findWhere($scope.cellTypes, {label: SPAWNER_CELL});
    });
    _.each(mapJsonFromFile.cells.goals, function(cell){
      findCell(cell.x, cell.y).type = _.findWhere($scope.cellTypes, {label: GOAL_CELL});
    });
  }

  function findCell(x, y){
    return _.findWhere(_.flatten($scope.mapModel.cells), {x: x, y: y});
  }

  function loadMap(mapJsonFromFile){
    initMapData(mapJsonFromFile);
  }

  function initCellMatrix(keepOldData){
    var oldCells = $scope.mapModel.cells;
    $scope.mapModel.cells = [];
    for(var i=0; i< $scope.mapModel.width; i++) {
      $scope.mapModel.cells[i] = new Array($scope.mapModel.height);
    }

    //init cells
    _.each($scope.mapModel.cells, function(column, x){
      _.each(column, function(cell, y){
        var cellType;
        if (keepOldData){
          if (oldCells[x] !== undefined && oldCells[x][y] !== undefined){
            cellType = oldCells[x][y].type;
          }
        }
        column[y] = {
          x: x,
          y: y,
          type: cellType || $scope.cellTypes[0]
        }
      });
    });
  }

  function onCellClick(cell){
    var currentlySelectedCellType = _.findWhere($scope.cellTypes, {label: $scope.selectedCellTypeLabel});
    var emptyCellType = _.findWhere($scope.cellTypes, {label: EMPTY_CELL});
    if (cell.type.label === currentlySelectedCellType.label){
      cell.type = emptyCellType;
    } else {
      cell.type = currentlySelectedCellType;
    }
  }

  function setUpDragFromDesktop(){

    var holder = document.body;

    holder.ondragover = function() {
        this.className = 'hover';
        return false;
    };
    holder.ondragend = function() {
        this.className = '';
        return false;
    };
    holder.ondrop = function(e) {
        this.className = '';
        e.preventDefault();

        var file = e.dataTransfer.files[0],
            reader = new FileReader();
        var fileName = file.name.split(".")[0];
        reader.onload = function(event, file) {
            $scope.mapModel.fileName = fileName;
            var map = JSON.parse(event.target.result);
            loadMap(map);
            $scope.$apply();
        };
        reader.readAsText(file);

        return false;
    };
  }

  function setUpDownloadBtn(){
    $scope.$watch('mapModel', function(){
      localStorage.setItem("map", JSON.stringify($scope.mapModel));
      assignFileToButton();
    }, true);
  }

  function assignFileToButton(){
    setTimeout(function(){
      var link = document.querySelector("#map-download-btn a");
      link.href = makeTextFile(JSON.stringify(getMapJson(), null, 2));
    }, 250);
  }

  function makeTextFile (text) {
      var data = new Blob([text], {type: 'text/plain'});
      // If we are replacing a previously generated file we need to
      // manually revoke the object URL to avoid memory leaks.
      if (textFile !== null) {
        window.URL.revokeObjectURL(textFile);
      }
      textFile = window.URL.createObjectURL(data);
      return textFile;
  }

  function getMapJson(){
    var mapJson = {
      dimensions: {
        width: $scope.mapModel.width,
        height: $scope.mapModel.height
      },
      cells: {
        walls: [],
        spawners: [],
        goals: [],
        damageBoosters: []
      }
    };
    _.each(_.flatten($scope.mapModel.cells), function(cell){
      var cellToPush = {
        x: cell.x,
        y: cell.y
      };
      if (cell.type.label === OBSTACLE_CELL){
        mapJson.cells.walls.push(cellToPush);
      } else if (cell.type.label === SPAWNER_CELL){
        mapJson.cells.spawners.push(cellToPush);
      }else if (cell.type.label === GOAL_CELL){
        mapJson.cells.goals.push(cellToPush);
      }else if (cell.type.label === DAMAGE_BOOST_CELL){
        mapJson.cells.damageBoosters.push(cellToPush);
      }
    });
    return mapJson;
  }

  function copyMapToClipboard(){
    copyToClipboard(JSON.stringify(getMapJson(), null, 2));
  }

  function copyToClipboard(text) {
    window.prompt("Copy to clipboard: Ctrl+C, Enter", text);
  }
});
