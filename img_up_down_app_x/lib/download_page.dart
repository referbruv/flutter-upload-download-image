import 'dart:convert';
import 'dart:typed_data';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class DownloadPage extends StatefulWidget {
  final String title;
  DownloadPage({this.title});

  @override
  State<StatefulWidget> createState() {
    return _DownloadPageState();
  }
}

class _DownloadPageState extends State<DownloadPage> {
  Future<String> _fetchImageData(String fileName) async {
    final String baseUri = 'https://<myapi>.azurewebsites.net';
    final response = await http.get('$baseUri/api/Assets/$fileName');
    if (response.statusCode == 200) {
      return response.body;
    } else {
      throw Exception('Failed to fetch image');
    }
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Image"),
      ),
      body: FutureBuilder(
        future: _fetchImageData('637494042252764417.png'),
        builder: (fctx, snapshot) {
          if (snapshot.hasData) {
            Uint8List bytes = base64Decode(snapshot.data);
            return Container(
              height: 200,
              decoration: BoxDecoration(
                  color: Colors.green,
                  image: DecorationImage(image: MemoryImage(bytes))),
            );
          }
          return CircularProgressIndicator();
        },
      ),
    );
  }
}
