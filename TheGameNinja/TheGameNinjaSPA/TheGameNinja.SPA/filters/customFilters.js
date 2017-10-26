angular.module("customFilters", [])
    .filter("unique", function () {
        return function (data, propertyName) {
            if (angular.isArray(data) && angular.isString(propertyName)) {
                var results = [];
                var keys = {};
                for (var i = 0; i < data.length; i++) {
                    var val = data[i][propertyName];
                    if (angular.isUndefined(keys[val])) {
                        keys[val] = true;
                        results.push(val);
                    }
                }
                return results;
            } else {
                return data;
            }
        }
    })
    .filter("range", function ($filter) {
        return function (data, page, size) {
            if (angular.isArray(data) && angular.isNumber(page) && angular.isNumber(size)) {
                var start_index = (page - 1) * size;
                if (data.length < start_index) {
                    return [];
                } else {
                    return $filter("limitTo")(data.splice(start_index), size);
                }
            } else {
                return data;
            }
        }
    })
    .filter("pageCount", function () {
        return function (data, size) {
            if (angular.isArray(data)) {
                var result = [];
                for (var i = 0; i < Math.ceil(data.length / size) ; i++) {
                    result.push(i);
                }
                return result;
            } else {
                return data;
            }
        }
    })
    .filter('cut', function () {
        return function (value, wordwise, max, tail) {
            if (!value) return '';

            max = parseInt(max, 10);
            if (!max) return value;
            if (value.length <= max) return value;

            value = value.substr(0, max);
            if (wordwise) {
                var lastspace = value.lastIndexOf(' ');
                if (lastspace != -1) {
                    value = value.substr(0, lastspace);
                }
            }

            return value + (tail || ' …');
        };
    });