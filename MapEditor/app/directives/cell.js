mapEditor.directive("gdCell", function(){
  return {
    restrict: "E",
    scope: {
      cell: "=model",
      onClick: "&"
    },
    templateUrl: "app/views/cell.html",
    link: function(scope, element, attrs){

      updateStyleObj();
      scope.$watch('cell', function(){
        updateStyleObj();
      }, true);

      function updateStyleObj(){
        var color = "white";
        if (scope.cell.isObstacle){
          color = "red";
        }
        scope.styleObj = {'background-color': color};
      }
    }
  }
})
