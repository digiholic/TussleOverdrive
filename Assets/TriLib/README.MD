#### Basic usage

Synchronous model loading:
```
using (var assetLoader = new AssetLoader()) {
    var assetLoaderOptions = AssetLoaderOptions.CreateInstance();   //Creates the AssetLoaderOptions instance.
                                                                    //AssetLoaderOptions let you specify options to load your model.
                                                                    //(Optional) You can skip this object creation and it's parameter or pass null.
    
    //You can modify assetLoaderOptions before passing it to LoadFromFile method. You can check the AssetLoaderOptions API reference at:
    //https://ricardoreis.net/trilib/manual/html/class_tri_lib_1_1_asset_loader_options.html
    
    var wrapperGameObject = gameObject;                             //Sets the game object where your model will be loaded into.
                                                                    //(Optional) You can skip this object creation and it's parameter or pass null.

    var myGameObject = assetLoader.LoadFromFile("PATH TO MY FILE.FBX", assetLoaderOptions, wrapperGameObject); //Loads the model synchronously and stores the reference in myGameObject.
}
```
Asynchronous model loading:
```
using (var assetLoaderAsync = new AssetLoaderAsync()) {
    var assetLoaderOptions = AssetLoaderOptions.CreateInstance();   //Creates the AssetLoaderOptions instance.
                                                                    //AssetLoaderOptions let you specify options to load your model.
                                                                    //(Optional) You can skip this object creation and it's parameter or pass null.
    
    //You can modify assetLoaderOptions before passing it to LoadFromFile method. You can check the AssetLoaderOptions API reference at:
    //https://ricardoreis.net/trilib/manual/html/class_tri_lib_1_1_asset_loader_options.html
    
    var wrapperGameObject = gameObject;                             //Sets the game object where your model will be loaded into.
                                                                    //(Optional) You can skip this object creation and it's parameter or pass null.

    var thread = assetLoaderAsync.LoadFromFile("PATH TO MY FILE.FBX", assetLoaderOptions, wrapperGameObject, delegate(GameObject myGameObject) {
        //Here you can get the reference to the loaded model using myGameObject.
    }); //Loads the model asynchronously and returns the reference to the created Task/Thread.
}
```
Retrieving unit scale factor from FBX models:
```
using (var assetLoader = new AssetLoader()) {
    var unitScaleFactor = 100f; //Sets scale factor default value.
    assetLoader.OnMetadataProcessed += delegate (AssimpMetadataType metadataType, uint metadataIndex, string metadataKey, object metadataValue) {
        //Checks if the metadata key is "UnitScaleFactor", which is the scale factor metadata used in FBX files.
        if (metadataKey == "UnitScaleFactor")
        {
            unitScaleFactor = (float)metadataValue; //Sets the scale factor value.
        }
    }; //Sets a callback to process model metadata.
    var myGameObject = assetLoader.LoadFromFile("PATH TO MY FILE.FBX"); //Loads the model synchronously and stores the reference in myGameObject.
    myGameObject.transform.localScale = Vector3.one \* (unitScaleFactor / 100f); //Uses the scale factor on the loaded model.
}
```