// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var highlightFields = function (response) {

    $('.form-group').removeClass('has-error');

    $.each(response, function (propName, val) {
        var nameSelector = '[name = "' + propName.replace(/(:|\.|\[|\])/g, "\\$1") + '"]',
            idSelector = '#' + propName.replace(/(:|\.|\[|\])/g, "\\$1");
        var $el = $(nameSelector) || $(idSelector);

        if (val.Errors.length > 0) {
            $el.closest('.form-group').addClass('has-error');
        }
    });
};
var highlightErrors = function (xhr) {
    try {
        var data = JSON.parse(xhr.responseText);
        highlightFields(data);
        showSummary(data);
        window.scrollTo(0, 0);
    } catch (e) {
        // (Hopefully) caught by the generic error handler in `config.js`.
    }
};
var showSummary = function (response) {
    $('#validationSummary').empty().removeClass('hidden');

    var verboseErrors = _.flatten(_.map(response, 'Errors')),
        errors = [];

    var nonNullErrors = _.reject(verboseErrors, function (error) {
        return error.ErrorMessage.indexOf('must not be empty') > -1;
    });

    _.each(nonNullErrors, function (error) {
        errors.push(error.ErrorMessage);
    });

    if (nonNullErrors.length !== verboseErrors.length) {
        errors.push('The highlighted fields are required to submit this form.');
    }

    var $ul = $('#validationSummary').append('<ul></ul>');

    _.each(errors, function (error) {
        var $li = $('<li></li>').text(error);
        $li.appendTo($ul);
    });
};

var redirect = function (data) {
    if (data.redirect) {
        window.location = data.redirect;
    } else {
        window.scrollTo(0, 0);
        window.location.reload();
    }
};

$('form[method=post]').not('.no-ajax').on('submit', function () {
    var submitBtn = $(this).find('[type="submit"]');

    submitBtn.prop('disabled', true);
    $(window).unbind();

    var $this = $(this),
        formData = $this.serialize();

    $this.find('div').removeClass('has-error');

    $.ajax({
        url: $this.attr('action'),
        type: 'post',
        data: formData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: 'json',
        statusCode: {
            200: redirect
        },
        complete: function () {
            submitBtn.prop('disabled', false);
        }
    }).fail(highlightErrors);

    return false;
});