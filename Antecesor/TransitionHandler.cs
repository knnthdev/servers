
public struct TransHandler
{
    public TransHandler(int mintop) // mintop es la position actual
    {
        _mintop = mintop;
        _index = mintop;// index es la position anterior
        _completed = true;
    }
    int _mintop;
    int _index;
    bool _completed;
    public void SetPos(int current) => _index = current;
    public int Push()
    {
        if (_index > 0)
            if (_index >= _mintop)
                --_index;
        _completed = _index == _mintop;
        return _index;
    }

    public bool Completed { get => _completed; }
    public int Put { get => Push(); }
    public int Mintop { get => _mintop; set => _mintop = value; }
}

