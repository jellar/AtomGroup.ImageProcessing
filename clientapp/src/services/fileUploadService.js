import http from '../http-common';

const uploadService = (file, width, color, watermarkText, onUploadProgress) => {
    let formData = new FormData();
    formData.append("file", file);
    formData.append("width", width);
    formData.append("backGroundColor", color);
    formData.append("watermarkText", watermarkText);
    formData.append("xdpi", "300");
    formData.append("ydpi", "300");
    formData.append("extension", ".jpeg");
    

    return http.post("image/upload", formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
        onUploadProgress,
    });
}

export default uploadService