import React, { useState, useEffect } from "react";
import uploadService from "../services/fileUploadService";

const FileUpload = () => {
    const [formValues, setFormValues] = useState({
        width: '',
        backGroundColor: '',
        watermarkText: '',
    });
    
    const [selectedFiles, setSelectedFiles] = useState(undefined);
    const [currentFile, setCurrentFile] = useState(undefined);
    
    const [progress, setProgress] = useState(0);
    const [message, setMessage] = useState("");
    const [processed, setProcessed] = useState(false);
    const [processedImage, setProcessedImage ] = useState({"fileContents": null, "fileDownloadName": null });
    
    const selectFile = (event) => {
        setSelectedFiles(event.target.files);
    };
    
    const handleWidthInputChange = (event) => {
        event.persist();
        setFormValues((values) => ({
            ...values,
            width: event.target.value,
        }));
    };

    const handleBackGroundInputChange = (event) => {
        event.persist();
        setFormValues((values) => ({
            ...values,
            backGroundColor: event.target.value,
        }));
    };

    const handleWatermarkTextInputChange = (event) => {
        event.persist();
        setFormValues((values) => ({
            ...values,
            watermarkText: event.target.value,
        }));
    };
    
    const upload = () => {
        let currentFile = selectedFiles[0];

        setProgress(0);
        setCurrentFile(currentFile);
        
        uploadService(currentFile, formValues.width, formValues.backGroundColor, 
            formValues.watermarkText, (event) => {
            console.log(formValues);
            setProgress(Math.round((100 * event.loaded) / event.total));
        })
            .then((response) => {
                setProcessed(true);
                setProcessedImage({...response.data});
            })            
            .catch(() => {
                setProgress(0);
                setMessage("Could not upload the file!");
                setCurrentFile(undefined);
            });

        setSelectedFiles(undefined);
    };
    return (
        <div>
            {currentFile && (
                <div className="progress">
                    <div
                        className="progress-bar progress-bar-info progress-bar-striped"
                        role="progressbar"
                        aria-valuenow={progress}
                        aria-valuemin="0"
                        aria-valuemax="100"
                        style={{ width: progress + "%" }}
                    >
                        {progress}%
                    </div>
                </div>
            )}
            <form>
                <div className="mb-3 col-4">
                    <label className="btn btn-default"> 
                        <input type="file" onChange={selectFile} /> 
                    </label>
                </div>    
                <div className="col-4">
                    <label className="form-label">Width *</label>
                    <input type="text" className="form-control" id="inputWidth" required value={formValues.width}
                    onChange={handleWidthInputChange}/>
                </div>  
                <div className="col-4">
                    <label className="form-label">BackGround Color</label>
                    <input type="text" className="form-control" id="inputBackgroundColor" value={formValues.backGroundColor}
                    onChange={handleBackGroundInputChange}/>
                </div>
                <div className="col-4">
                    <label className="form-label">Watermark Text</label>
                    <input type="text" className="form-control" id="inputWatermarkText" value={formValues.watermarkText}
                    onChange={handleWatermarkTextInputChange}/>
                </div>
                <div className="col-4"><br/>
                <button type="submit"
                    className="btn btn-success"
                    disabled={!selectedFiles}
                    onClick={upload}
                >
                    Upload
                </button></div>
            </form>    

            <div className="alert alert-light" role="alert">
                {message}
            </div>
            {processed && 
            <div className="row">
                <div className="p-2">
                    <h4>Original Image</h4>
                    <OrinalImage image={currentFile}/>
                </div>
                <div className="p-2">
                    <h4>Processed Image</h4>
                    <img src={`data:image/jpeg;base64,${processedImage.fileContents}`} alt={processedImage.fileDownloadName} />
                </div>
                
            </div>
           }
           
        </div>
    );
}

export default FileUpload;

const OrinalImage = ({ image }) => {
    return <img src={URL.createObjectURL(image)} alt={image.name} />;
};