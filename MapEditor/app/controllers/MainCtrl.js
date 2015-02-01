mapEditor.controller("MainCtrl", function($scope){

  var textFile = null;

  init();

  function init(){
    initMapData(null);

    $scope.onCellClick = onCellClick;
    $scope.copyMapToClipboard = copyMapToClipboard;
    setUpDragFromDesktop();
    setUpDownloadBtn();
  }

  function initMapData(mapObj){
    $scope.mapModel = {
      width: mapObj ? mapObj.dimensions.width : 15,
      height: mapObj ? mapObj.dimensions.height : 10
    };
    initCellMatrix();
    if (mapObj){
      _.each(mapObj.cells.walls, function(cell){
        findCell(cell.x, cell.y).isObstacle = true;
      });
    }
  }

  function findCell(x, y){
    return _.findWhere(_.flatten($scope.cells), {x: x, y: y});
  }

  function loadMap(mapObj){
    initMapData(mapObj);
  }

  function initCellMatrix(){
    $scope.cells = [];
    for(var i=0; i< $scope.mapModel.width; i++) {
      $scope.cells[i] = new Array($scope.mapModel.height);
    }

    //init cells
    _.each($scope.cells, function(column, x){
      _.each(column, function(cell, y){
        column[y] = {
          x: x,
          y: y,
          isObstacle: false
        }
      });
    });
  }

  function onCellClick(cell){
    cell.isObstacle = !cell.isObstacle;
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
        reader.onload = function(event) {
            var map = JSON.parse(event.target.result);
            loadMap(map);
            $scope.$apply();
        };
        reader.readAsText(file);

        return false;
    };
  }

  function setUpDownloadBtn(){
    $scope.$watch('cells', function(){
      assignFileToButton();
    }, true);
    $scope.$watch('mapModel', function(){
      assignFileToButton();
    }, true);
  }

  function assignFileToButton(){
    setTimeout(function(){
      var link = document.getElementById('map-download-btn');
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
        walls: []
      }
    }
    _.each(_.flatten($scope.cells), function(cell){
      if (cell.isObstacle){
        mapJson.cells.walls.push({
          x: cell.x,
          y: cell.y
        });
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
