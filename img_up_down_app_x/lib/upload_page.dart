// import 'package:flutter/cupertino.dart';
// import 'package:flutter/material.dart';
// import 'package:file_picker/file_picker.dart';
// import 'package:path/path.dart';
// import 'dart:io';
// import 'package:http/http.dart' as http;
// import 'dart:convert';
// import 'package:fluttertoast/fluttertoast.dart';

// class UploadPage extends StatefulWidget {
//   @override
//   State<StatefulWidget> createState() {
//     return _UploadPageState();
//   }
// }

// class _UploadPageState extends State<UploadPage> {
//   final String baseUri = 'https://<myapi>.azurewebsites.net';
//   String imgUri = '';
//   Future<http.StreamedResponse> _uploadToApi(File file) async {
//     // open a bytestream
//     var stream = new http.ByteStream(file.openRead());
//     // get file length
//     var length = await file.length();

//     // string to uri
//     var uri = Uri.parse("$baseUri/api/Assets/upload");

//     // create multipart request
//     var request = new http.MultipartRequest("POST", uri);

//     // multipart that takes file
//     var multipartFile = new http.MultipartFile('file', stream, length,
//         filename: basename(file.path));

//     // add file to multipart
//     request.files.add(multipartFile);

//     // send
//     return await request.send();
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(
//         title: Text('Upload'),
//       ),
//       body: Container(
//         child: RaisedButton(
//           child: Text('Upload Images'),
//           onPressed: () async {
//             FilePickerResult result = await FilePicker.platform.pickFiles(
//               type: FileType.custom,
//               allowedExtensions: ['jpg', 'png'],
//             );
//             if (result != null) {
//               File file = File(result.files.single.path);
//               var response = await _uploadToApi(file);
//               print(response.statusCode);
//               // listen for response
//               response.stream.transform(utf8.decoder).listen((value) {
//                 print(value);
//                 Fluttertoast.showToast(msg: value);
//                 imgUri = value;
//               });
//             }
//           },
//         ),
//       ),
//     );
//   }
// }
