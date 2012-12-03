var console;

if (window.external) {
    console = {};
    console.log = function (message) {
        window.external.notify(message);
    }
}