var Custom = {
    AddDragabbleElementClassByClassName: function (divClass,enableElementClass,classTobeAdd) {
        
        this.RemoveDragabbleElementClassByClassName(divClass, classTobeAdd);

        $('.' + enableElementClass).addClass(classTobeAdd);

    },
    RemoveDragabbleElementClassByClassName: function (taskItemDivClass,classTobeRemove) {
        
        $('.' + taskItemDivClass).each(function (index, value) {
            $(this).removeClass(classTobeRemove);
        });
    }

};