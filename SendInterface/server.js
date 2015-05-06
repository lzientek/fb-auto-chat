var http = require('http');
var login = require('facebook-chat-api');
var fs = require('fs');
var Api;

var port = process.env.port || 1337;


http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    var body = '';
    req.on('data', function (data) { body += data });
    req.on('end', function () {
        
        
        if (req.url === "/set") {
            
            fs.writeFileSync('opt.json', body);
            login('opt.json', function (error, api) {
                Api = api;
            });
        } else if (req.url === "/sendMessage" && Api) {
            var val = JSON.parse(body);
            Api.sendMessage(val.message,val.threadId, function(parameters) {
                console.log(parameters);
            });
        }
    });
    
    res.end('');
}).listen(port);